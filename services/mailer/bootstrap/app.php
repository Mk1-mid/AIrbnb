<?php

require __DIR__ . '/../vendor/autoload.php';

use Illuminate\Container\Container;
use Illuminate\Events\EventServiceProvider;
use Illuminate\Routing\Router;
use Illuminate\Filesystem\FilesystemServiceProvider;
use Illuminate\Log\LogServiceProvider;
use Illuminate\Mail\MailServiceProvider;

$app = new Container();
$app->singleton('config', function () {
    return new Illuminate\Config\Repository([
        'app' => [
            'debug' => true,
        ],
        'mail' => require __DIR__ . '/../config/mail.php',
        'logging' => [
            'default' => 'stack',
            'channels' => [
                'stack' => [
                    'driver' => 'stack',
                    'channels' => ['single'],
                ],
                'single' => [
                    'driver' => 'single',
                    'path' => __DIR__ . '/../storage/logs/laravel.log',
                ],
            ],
        ],
        'services' => [
            'mailgun' => [
                'domain' => env('MAILGUN_DOMAIN'),
                'secret' => env('MAILGUN_SECRET'),
                'endpoint' => env('MAILGUN_ENDPOINT', 'api.mailgun.net'),
            ],
        ],
    ]);
});

$app->register(new EventServiceProvider($app));
$app->register(new FilesystemServiceProvider($app));
$app->register(new LogServiceProvider($app));
$app->register(new MailServiceProvider($app));

return $app;
