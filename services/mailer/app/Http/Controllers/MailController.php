<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Mail\Message;
use Illuminate\Routing\Controller;
use Illuminate\Support\Facades\Mail;

class MailController extends Controller
{
    public function send(Request $request)
    {
        $request->validate([
            'to' => 'required|email',
            'subject' => 'required|string|max:255',
            'body' => 'required|string',
        ]);

        Mail::send([], [], function (Message $message) use ($request) {
            $message->to($request->input('to'))
                ->subject($request->input('subject'))
                ->setBody($request->input('body'));
        });

        return response()->json(['status' => 'sent'], 200);
    }
}
