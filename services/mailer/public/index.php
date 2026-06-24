<?php

require __DIR__ . '/../vendor/autoload.php';

use Symfony\Component\Mailer\Mailer;
use Symfony\Component\Mailer\Transport\Smtp\EsmtpTransportFactory;
use Symfony\Component\Mime\Email;
use Symfony\Component\Mime\Address;

$method = $_SERVER['REQUEST_METHOD'] ?? 'GET';
$path = parse_url($_SERVER['REQUEST_URI'] ?? '/', PHP_URL_PATH) ?? '/';

if ($path === '/send-email' && $method === 'POST') {
    $body = file_get_contents('php://input');
    $data = json_decode($body, true);

    if (empty($data['to']) || empty($data['subject']) || empty($data['body'])) {
        http_response_code(400);
        header('Content-Type: application/json');
        echo json_encode(['status' => 'error', 'message' => 'Missing fields: to, subject, body']);
        exit;
    }

    $transport = (new EsmtpTransportFactory())->create(
        host: getenv('MAIL_HOST') ?: 'live.smtp.mailtrap.io',
        port: (int) (getenv('MAIL_PORT') ?: 587),
        encryption: getenv('MAIL_ENCRYPTION') ?: 'tls',
        username: getenv('MAIL_USERNAME') ?: 'api',
        password: (string) (getenv('MAIL_PASSWORD') ?: '')
    );

    $mailer = new Mailer($transport);

    $email = (new Email())
        ->from(new Address(getenv('MAIL_FROM_ADDRESS') ?: 'noreply@rentalplatform.com', getenv('MAIL_FROM_NAME') ?: 'RentalPlatform'))
        ->to(new Address($data['to']))
        ->subject($data['subject'])
        ->text($data['body']);

    try {
        $mailer->send($email);
    } catch (Throwable $e) {
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
