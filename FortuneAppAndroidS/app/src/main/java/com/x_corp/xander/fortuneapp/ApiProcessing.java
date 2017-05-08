package com.x_corp.xander.fortuneapp;

import android.os.AsyncTask;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;

/**
 * Created by Xander on 5/8/2017.
 */

public class ApiProcessing extends AsyncTask<String, Integer, String> {
    public static String makeItProper(String toBeEnchanted) {
        String toBeReturned = toBeEnchanted.replace("\\t", "\n");

        toBeReturned = toBeReturned.replace("\\n", "\n");
        toBeReturned = toBeReturned.replace("\\", "");
        toBeReturned = toBeReturned.replace("Q:\n", "Q:\t");
        toBeReturned = toBeReturned.replace("A:\n", "A:\t");
        toBeReturned = toBeReturned.replace("\n\n", "\n");

        toBeReturned = toBeReturned.substring(1, toBeReturned.length() - 2);

        if (toBeReturned.length() > 800) {
            toBeReturned = toBeReturned.substring(0, 800);
        }

        if (toBeReturned.equals("Rate Limit Exceeded")) {
            toBeReturned = "Memento Mori!";
        }

        if (toBeReturned.length() == 0) {
            toBeReturned = "We need internet for our relationship to Work!";
        }

        return toBeReturned;
    }

    private static void doNothingElse() {
    }

    private static void doNothing() {
    }

    @Override
    protected String doInBackground(String... params) {
        URL url = null;
        try {
            url = new URL(params[0]);
        } catch (MalformedURLException e) {
            e.printStackTrace();
        }

        HttpURLConnection connection = null;
        try {
            connection = (HttpURLConnection) url.openConnection();
            connection.setRequestMethod("GET");
            connection.setDoOutput(true);
            connection.setConnectTimeout(5000);
            connection.setReadTimeout(5000);
            connection.connect();
        } catch (IOException e) {
            e.printStackTrace();
        }
        String content = "", line;
        BufferedReader rd = null;
        try {
            rd = new BufferedReader(new InputStreamReader(connection.getInputStream()));
        } catch (IOException e) {
            e.printStackTrace();
        }
        try {
            while ((line = rd.readLine()) != null) {
                content += line + "\n";
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
        return content;
    }
}
