package com.siddg.uwroomfinder;

import android.app.ActionBar;
import android.app.ProgressDialog;
import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.ActionBarActivity;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ListView;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by Sidd on 12/18/2014.
 */
public class FloorActivity extends ActionBarActivity {

    private ListView floorList;
    private List<String> floors;
    private ArrayAdapter<String> floorAdapter;
    private ProgressDialog loadingFeedback;

    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_floor);
        String currentBuilding = getIntent().getStringExtra("buildingName");

        floorList = (ListView) findViewById(R.id.floorList);
        /*floorList.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                String floorNumber = floors.get(position);
                Intent i = new Intent(FloorActivity.this, FloorActivity.class);
                i.putExtra("floor", floorNumber);
                startActivity(i);

            }
        }); */
        floors = new ArrayList<String>();
        floorAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_list_item_1, floors);
        floorList.setAdapter(floorAdapter);
        // Parse Query to find floor numbers



    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
        return true;
    }

    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        // Refresh action
        if(id == R.id.action_refresh) {
            //getFloors();
        }


        return super.onOptionsItemSelected(item);
    }
}
