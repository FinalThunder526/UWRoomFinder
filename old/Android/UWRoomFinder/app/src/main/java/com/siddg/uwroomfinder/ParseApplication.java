package com.siddg.uwroomfinder;

import android.app.Application;

import com.parse.Parse;

/**
 * Created by Sidd on 12/18/2014.
 */
public class ParseApplication extends Application {
    public void onCreate() {
        // Enable Local Datastore.
        Parse.enableLocalDatastore(this);

        Parse.initialize(this, "L3Toj2w8dfPshDHJX1sqDWVcT4enSvOusLnxJo5f", "pTVkoYOJJrcShql5bqLrmhgaenN5fXvES8G2nEWQ");
    }
}
