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
import android.widget.Toast;

import com.parse.FindCallback;
import com.parse.ParseException;
import com.parse.ParseObject;
import com.parse.ParseQuery;

import java.util.ArrayList;
import java.util.List;
import java.util.Set;
import java.util.TreeSet;

/**
 * Created by Sidd on 12/18/2014.
 */
public class FloorActivity extends ActionBarActivity {

    private ListView floorList;
    private List<String> floors;
    private ArrayAdapter<String> floorAdapter;
    private ProgressDialog loadingFeedback;
    private String currentBuilding;

    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_floor);
        currentBuilding = getIntent().getStringExtra("buildingName");

        floorList = (ListView) findViewById(R.id.floorList);
        floorList.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                String floorNumber = floors.get(position);
                String n = floorNumber.substring(6);
                Intent i = new Intent(FloorActivity.this, RoomActivity.class);
                i.putExtra("floor", n);
                i.putExtra("currentBuilding", currentBuilding);
                startActivity(i);

            }
        });
        floors = new ArrayList<String>();
        floorAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_list_item_1, floors);
        floorList.setAdapter(floorAdapter);
        // Parse Query to find floor numbers
        getFloors();



    }

    private void getFloors() {
        ParseQuery<ParseObject> query = ParseQuery.getQuery("StudyRoom");
        loadingFeedback = ProgressDialog.show(this, "", "Loading " + currentBuilding + " floors");
        query.whereEqualTo("buildingName", currentBuilding);
        query.findInBackground(new FindCallback<ParseObject>() {
            @Override
            public void done(List<ParseObject> parseObjects, ParseException e) {
                if (e == null) {
                    Set<String> Floors = new TreeSet<String>();
                    for (ParseObject object : parseObjects) {
                        String floors = object.getString("roomNumber");
                        String floorName = "Floor " + floors.substring(0,1);
                        Floors.add(floorName);

                    }
                    addFloorstoList(Floors);
                } else {
                    // Failed result
                    Toast.makeText(FloorActivity.this,
                            "Failed to load floors. Make sure you are connected to a network",
                            Toast.LENGTH_LONG).show();
                }
            }
        });

    }

    private void addFloorstoList(Set<String> Floors) {
        floors.clear();
        for (String s : Floors) {
            floors.add(s);
        }
        floorAdapter.notifyDataSetChanged();
        loadingFeedback.dismiss();
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
            getFloors();
        }


        return super.onOptionsItemSelected(item);
    }


}
