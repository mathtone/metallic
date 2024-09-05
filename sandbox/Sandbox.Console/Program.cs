using Sandbox.Console;
using ILogger = Microsoft.Extensions.Logging.ILogger;

var app = Host
	.CreateDefaultBuilder()
	.ConfigureServices(svc => svc
		.Configure<ConsoleLifetimeOptions>(options => options.SuppressStatusMessages = true)
		.AddSerilog((svc, cfg) => cfg.WriteTo.Console(theme: LogThemes.Subtle))
		.AddSingleton(GetConfig)
		.AddScoped<ModuleHostService>()
		.AddHostedService(svc => svc.GetRequiredService<ModuleHostService>())
	)
	.Build();

await app.StartAsync();

var svc = app.Services
	.GetRequiredService<ModuleHostService>()
	.Services
	.GetRequiredService<ITestService>();
svc.Test();

await app.StopAsync();

static Task<ModuleHostConfiguration> GetConfig() => Task.FromResult(
	new ModuleHostConfiguration() {
		Name = "Test",
		Args = Environment.GetCommandLineArgs(),
		Modules = [
			new() {
				Name = "Logging Module",
				Initializers = [typeof(TestServiceCollectionInitializer)]
			},
			new() {
				Name = "Test Module",
				Services = ServiceConfiguration.CreateDefault()
					.Singleton<ITestService,TestService>()
			}
		]
	});

namespace Sandbox.Console {
	using CFGLIST = List<ServiceConfiguration>;

	public class TestService(ILogger<TestService> logger) : ITestService {
		readonly ILogger Logger = logger;

		public void Test() => Logger.Print("Test Service Executing Test");
	}
	public interface ITestService {
		void Test();
	}

	public interface IServiceCollectionInitializer {
		void Initialize(IServiceCollection services);
	}

	public abstract class ServiceCollectionInitializer : IServiceCollectionInitializer {
		public abstract void Initialize(IServiceCollection services);
	}

	public class TestServiceCollectionInitializer : ServiceCollectionInitializer {
		public override void Initialize(IServiceCollection services) => services
			.AddSerilog((svc, cfg) => cfg.WriteTo.Console(theme: LogThemes.Vainglorious));
	}

	public class ModuleHostService(ILogger<ModuleHostService> logger, Func<Task<ModuleHostConfiguration>> configLocator) :
		ModuleHost(configLocator), IHostedService {
		protected ILogger Logger = logger;

		protected override void OnLoadModule(ModuleConfiguration configuration) {
			base.OnLoadModule(configuration);
			logger.LoadingModule(configuration.Name);
		}

		protected override void OnInitializeModule(IServiceCollectionInitializer initializer) {
			base.OnInitializeModule(initializer);
			logger.InitModule(initializer.ToString()!);
		}
	}

	public class ModuleHost(Func<Task<ModuleHostConfiguration>> configLocator) : IHost, IAsyncDisposable {

		IHost? app;
		public IServiceProvider Services => app!.Services;

		public void Dispose() {
			OnDispose();
			GC.SuppressFinalize(this);
		}

		protected virtual void OnDispose() { }

		protected virtual ValueTask OnDisposeAsync() {
			OnDispose();
			return ValueTask.CompletedTask;
		}

		public async Task StartAsync(CancellationToken cancellationToken = default) {
			var cfg = await configLocator();
			var bldr = Host.CreateDefaultBuilder()
				.ConfigureServices(svc => {
					foreach (var module in cfg.Modules) {
						OnLoadModule(module);
						foreach (var service in module.Services) {
							foreach (var desc in service.GetServiceDescriptors()) {
								svc.Add(desc);
							}
						}
						foreach (var initializer in module.Initializers) {
							var i = Activator.CreateInstance(initializer) as IServiceCollectionInitializer;
							OnInitializeModule(i);
							i!.Initialize(svc);
						}
					}
				});
			app = bldr.Build();
			await app.StartAsync(cancellationToken);
		}

		protected virtual void OnLoadModule(ModuleConfiguration configuration) { }
		protected virtual void OnInitializeModule(IServiceCollectionInitializer initializer) { }

		public async Task StopAsync(CancellationToken cancellationToken = default) {
			if (app is not null) {
				await app.StopAsync(cancellationToken);
			}
		}

		public async ValueTask DisposeAsync() {
			await OnDisposeAsync();
			GC.SuppressFinalize(this);
		}
	}

	public class ModuleHostConfiguration {
		public string Name { get; set; } = "";
		public string[] Args { get; set; } = [];
		public List<ModuleConfiguration> Modules { get; set; } = [];
	}

	public class ModuleConfiguration {
		public string Name { get; set; } = "";
		public IList<ServiceConfiguration> Services { get; set; } = [];
		public List<Type> Initializers { get; set; } = [];
	}

	public class ServiceConfiguration {

		public ServiceLifetime? Lifetime { get; set; }
		public IList<Type> ServiceTypes { get; set; } = [];
		public Type ImplementationType { get; set; } = typeof(object);
		public bool AddHosting { get; set; }
		public Type PrimaryServiceType => ServiceTypes.FirstOrDefault() ?? ImplementationType;
		public IEnumerable<Type> SecondaryServiceTypes => ServiceTypes.Skip(1);
		public IEnumerable<object> ActivatorParams { get; set; } = [];

		public IEnumerable<ServiceDescriptor> GetServiceDescriptors() {

			if (Lifetime.HasValue) {
				yield return ActivatorParams.Any() ?
					  new(PrimaryServiceType, svc => CreateInstance(svc, ImplementationType, ActivatorParams.ToArray()), Lifetime.Value) :
					  new(PrimaryServiceType, ImplementationType, Lifetime.Value);

				foreach (var secondary in ServiceTypes.Skip(1)) {
					yield return new(secondary, svc => svc.GetRequiredService(PrimaryServiceType), Lifetime.Value);
				}

				if (AddHosting) {
					yield return new(typeof(IHostedService), svc => svc.GetRequiredService(PrimaryServiceType), ServiceLifetime.Transient);
				}
			}
			else {
				if (AddHosting) {
					yield return ActivatorParams.Any() ?
						new(typeof(IHostedService), svc => CreateInstance(svc, ImplementationType, ActivatorParams.ToArray()), ServiceLifetime.Transient) :
						new(typeof(IHostedService), ImplementationType, ServiceLifetime.Transient);
				}
			}
		}

		public static CFGLIST CreateDefault() => [];
	}

	public static class ServiceConfigCollectionExtensions {

		public static CFGLIST Host<IMPL>(this CFGLIST services) {
			services.Add(new() {
				ImplementationType = typeof(IMPL),
				AddHosting = true
			});
			return services;
		}

		public static CFGLIST Singleton<SVC>(this CFGLIST services, bool addHosting = false) =>
			services.Add<SVC>(ServiceLifetime.Singleton, addHosting);

		public static CFGLIST Singleton<SVC, IMPL>(this CFGLIST services, bool addHosting = false) =>
			services.Add<SVC, IMPL>(ServiceLifetime.Singleton, addHosting);

		public static CFGLIST Singleton<IMPL>(this CFGLIST services, IEnumerable<Type> types, bool addHosting = false) =>
			services.Add<IMPL>(ServiceLifetime.Singleton, types, addHosting);

		public static CFGLIST Scoped<SVC>(this CFGLIST services, bool addHosting = false) =>
			services.Add<SVC>(ServiceLifetime.Scoped, addHosting);

		public static CFGLIST Scoped<SVC, IMPL>(this CFGLIST services, bool addHosting = false) =>
			services.Add<SVC, IMPL>(ServiceLifetime.Scoped, addHosting);

		public static CFGLIST Scoped<IMPL>(this CFGLIST services, IEnumerable<Type> types, bool addHosting = false) =>
			services.Add<IMPL>(ServiceLifetime.Scoped, types, addHosting);

		public static CFGLIST Transient<SVC>(this CFGLIST services, bool addHosting = false) =>
			services.Add<SVC>(ServiceLifetime.Transient, addHosting);

		public static CFGLIST Transient<SVC, IMPL>(this CFGLIST services, bool addHosting = false) =>
			services.Add<SVC, IMPL>(ServiceLifetime.Transient, addHosting);

		public static CFGLIST Transient<IMPL>(this CFGLIST services, IEnumerable<Type> types, bool addHosting = false) =>
			services.Add<IMPL>(ServiceLifetime.Transient, types, addHosting);

		public static CFGLIST Add<SVC>(this CFGLIST services, ServiceLifetime lifetime, bool addHosting = false) =>
			services.Add<SVC, SVC>(lifetime, addHosting);

		public static CFGLIST Add<SVC, IMPL>(this CFGLIST services, ServiceLifetime lifetime, bool addHosting = false) {
			services.Add(new() {
				Lifetime = lifetime,
				ServiceTypes = { typeof(SVC) },
				ImplementationType = typeof(IMPL),
				AddHosting = addHosting
			});
			return services;
		}

		public static CFGLIST Add<IMPL>(this CFGLIST services, ServiceLifetime lifetime, IEnumerable<Type> types, bool addHosting = false) {
			services.Add(new() {
				Lifetime = lifetime,
				ServiceTypes = types.ToList(),
				ImplementationType = typeof(IMPL),
				AddHosting = addHosting
			});

			return services;
		}

		public static CFGLIST ActivateHosted<IMPL>(this CFGLIST services, params object[] parameters) =>
			services.Activate<IMPL>(null, Enumerable.Empty<Type>(), true, parameters);

		public static CFGLIST ActivateHosted<SVC, IMPL>(this CFGLIST services, ServiceLifetime? lifetime, params object[] parameters) =>
			services.Activate<IMPL>(lifetime, new[] { typeof(SVC) }, true, parameters);

		public static CFGLIST Activate<SVC, IMPL>(this CFGLIST services, ServiceLifetime? lifetime, params object[] parameters) =>
			services.Activate<IMPL>(lifetime, new[] { typeof(SVC) }, false, parameters);

		public static CFGLIST Activate<IMPL>(this CFGLIST services, ServiceLifetime? lifetime, params object[] parameters) =>
			services.Activate<IMPL, IMPL>(lifetime, false, parameters);

		public static CFGLIST Activate<IMPL>(this CFGLIST services, ServiceLifetime? lifetime, IEnumerable<Type> types, bool addHosting = false, params object[] parameters) {
			services.Add(new() {
				Lifetime = lifetime,
				ServiceTypes = types.ToList(),
				ImplementationType = typeof(IMPL),
				AddHosting = addHosting,
				ActivatorParams = parameters
			});
			return services;
		}
	}

	public static partial class LoggingExtensions {

		[LoggerMessage(LogLevel.Information, "Loading Module: {module}", EventId = 1)]
		public static partial void LoadingModule(this ILogger logger, string module);

		[LoggerMessage(LogLevel.Information, "Running Initializer:{initializer}", EventId = 2)]
		public static partial void InitModule(this ILogger logger, string initializer);

		[LoggerMessage(LogLevel.Information, "{message}", EventId = 0)]
		public static partial void Print(this ILogger logger, string message);
	}

	public static class LogThemes {
		public static SystemConsoleTheme Subtle { get; } = new SystemConsoleTheme(new Dictionary<ConsoleThemeStyle, SystemConsoleThemeStyle> {
			[ConsoleThemeStyle.Text] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.SecondaryText] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.TertiaryText] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.Invalid] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.Null] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.Name] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.String] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.Number] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.Boolean] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.Scalar] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.LevelVerbose] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.LevelDebug] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.LevelInformation] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.LevelWarning] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.LevelError] = new() {
				Foreground = ConsoleColor.White,
				Background = ConsoleColor.Red
			},
			[ConsoleThemeStyle.LevelFatal] = new() {
				Foreground = ConsoleColor.White,
				Background = ConsoleColor.Red
			}
		});

		public static SystemConsoleTheme Vainglorious { get; } = new SystemConsoleTheme(new Dictionary<ConsoleThemeStyle, SystemConsoleThemeStyle> {
			[ConsoleThemeStyle.Text] = new() {
				Foreground = ConsoleColor.White
			},
			[ConsoleThemeStyle.SecondaryText] = new() {
				Foreground = ConsoleColor.Gray
			},
			[ConsoleThemeStyle.TertiaryText] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.Invalid] = new() {
				Foreground = ConsoleColor.Red
			},
			[ConsoleThemeStyle.Null] = new() {
				Foreground = ConsoleColor.Cyan
			},
			[ConsoleThemeStyle.Name] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.String] = new() {
				Foreground = ConsoleColor.Gray
			},
			[ConsoleThemeStyle.Number] = new() {
				Foreground = ConsoleColor.Magenta
			},
			[ConsoleThemeStyle.Boolean] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.Scalar] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.LevelVerbose] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.LevelDebug] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.LevelInformation] = new() {
				Foreground = ConsoleColor.Green
			},
			[ConsoleThemeStyle.LevelWarning] = new() {
				Foreground = ConsoleColor.DarkGray
			},
			[ConsoleThemeStyle.LevelError] = new() {
				Foreground = ConsoleColor.White,
				Background = ConsoleColor.Red
			},
			[ConsoleThemeStyle.LevelFatal] = new() {
				Foreground = ConsoleColor.White,
				Background = ConsoleColor.Red
			}
		});
	}
}