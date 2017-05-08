package com.x_corp.xander.fortuneapp;

import android.media.MediaPlayer;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.text.method.ScrollingMovementMethod;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import java.util.concurrent.ExecutionException;

/**
 * An example full-screen activity that shows and hides the system UI (i.e.
 * status bar and navigation/system bar) with user interaction.
 */
public class FortuneApp extends AppCompatActivity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        final MediaPlayer hmmSound = MediaPlayer.create(this, R.raw.hmm);

        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_fortune_app);

        Button button = (Button) findViewById(R.id.dummy_button);
        button.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                hmmSound.start();

                TextView ourText = (TextView) findViewById(R.id.needIt);
                ourText.setMovementMethod(new ScrollingMovementMethod());

                String data = null;
                try {
                    data = new ApiProcessing().execute("https://helloacm.com/api/fortune/").get();
                } catch (InterruptedException e) {
                    e.printStackTrace();
                } catch (ExecutionException e) {
                    e.printStackTrace();
                    data = " NOT TODAY  ";
                }
                ourText.setText(ApiProcessing.makeItProper(data));
            }
        });
    }
}
