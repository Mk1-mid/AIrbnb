<?php

require __DIR__ . '/../vendor/autoload.php';

use GuzzleHttp\Client;
use GuzzleHttp\Exception\GuzzleException;

$method = $_SERVER['REQUEST_METHOD'] ?? 'GET';
$path = parse_url($_SERVER['REQUEST_URI'] ?? '/', PHP_URL_PATH) ?? '/';

if ($path === '/send-email' && $method === 'POST') {
    $contentType = $_SERVER['CONTENT_TYPE'] ?? '';
    $body = file_get_contents('php://input');
    $data = json_decode($body, true);

    if (empty($data['to']) || empty($data['subject']) || empty($data['body'])) {
        http_response_code(400);
        header('Content-Type: application/json');
        echo json_encode(['status' => 'error', 'message' => 'Missing fields: to, subject, body']);
        exit;
    }

    $client = new Client([
        'timeout' => 10,
        'headers' => [
            'Authorization' => 'Bearer 66567d83ddc8b67660288f04bc391655',
            'Content-Type' => 'application/json',
            'Accept' => 'application/json',
        ],
    ]);

    $payload = [
        'from' => [
            'email' => 'noreply@rentalplatform.com',
            'name' => 'RentalPlatform',
        ],
        'to' => [
            [
                'email' => $data['to'],
            ],
        ],
        'subject' => $data['subject'],
        'text' => $data['body'],
        'category' => 'RentalPlatform',
    ];

    try {
        $client->post('https://send.api.mailtrap.io/api/send', [
            'json' => $payload,
        ]);
    } catch (GuzzleException $e) {
        http_response_code(500);
        header('Content-Type: application/json');
        echo json_encode(['status' => 'error', 'message' => $e->getMessage()]);
        exit;
    }

    header('Content-Type: application/json');
    echo json_encode(['status' => 'sent']);
    exit;
}

http_response_code(404);
header('Content-Type: application/json');
echo json_encode(['status' => 'not found']);
