﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using Telegram.Bot;
using TelegramBot.ConsoleApp;

// Достаём из конфига объект и десериализуем его строчкой ниже в модель.
var filePath = File.ReadAllText($@"{Environment.CurrentDirectory}/BotConfig.json");
var botToken = JsonSerializer.Deserialize<BotConfigModel>(filePath)?.Token;

// Объект, отвечающий за постоянный жизненный цикл приложения
var host = new HostBuilder()
    .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
    .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
    .Build(); // Собираем

Console.WriteLine("Сервис запущен");

// Запускаем сервис
await host.RunAsync();

Console.WriteLine("Сервис остановлен");

void ConfigureServices(IServiceCollection services)
{
    // Регистрируем объект TelegramBotClient c токеном подключения
    services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(botToken ?? ""));
    // Регистрируем постоянно активный сервис бота
    services.AddHostedService<Bot>();
}