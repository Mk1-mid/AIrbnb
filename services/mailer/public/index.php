<?php

use Illuminate\Routing\Router;
use Illuminate\Http\Request;

require __DIR__ . '/../vendor/autoload.php';

$app = require __DIR__ . '/../bootstrap/app.php';

$router = new Router($app['events'], $app);

$router->post('/send-email', function (Request $request) {
    $request->validate([
        'to' => 'required|email',
        'subject' => 'required|string|max:255',
        'body' => 'required|string',
    ]);

    Mail::send([], [], function ($message) use ($request) {
        $message->to($request->input('to'))
            ->subject($request->input('subject'))
            ->setBody($request->input('body'));
    });

    return response()->json(['status' => 'sent'], 200);
});

$request = Illuminate\Http\Request::capture();
$response = $router->dispatch($request);
$response->send();
