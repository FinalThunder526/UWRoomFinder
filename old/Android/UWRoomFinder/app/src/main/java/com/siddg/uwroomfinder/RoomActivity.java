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
 * Created by Sidd on 12/22/2014.
 */
public class RoomActivity extends ActionBarActivity {

    private ListView roomList;
    private List<String> rooms;
    private ArrayAdapter<String> roomAdapter;
    private ProgressDialog loadingFeedback;
    private String currentFloor;
    private String currentBuilding;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_room);
        currentFloor = getIntent().getStringExtra("floorNumber");
        currentBuilding = getIntent().getStringExtra("currentBuilding");

        roomList = (ListView) findViewById(R.id.roomList);
        roomList.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override

            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                String roomNumber = rooms.get(position);
                Intent i = new Intent(RoomActivity.this, ReservationActivity.class);
                i.putExtra("room", roomNumber);
                startActivity(i);

            }
        });
        rooms = new ArrayList<String>();
        roomAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_list_item_1, rooms);
        roomList.setAdapter(roomAdapter);
        // Parse Query to find floor numbers
        getRooms();


    }


    private void getRooms() {
        ParseQuery<ParseObject> query = ParseQuery.getQuery("StudyRoom");
        loadingFeedback = ProgressDialog.show(this, "", "Loading study rooms on floor " + currentFloor);
        query.whereEqualTo("buildingName", currentBuilding);
        query.whereStartsWith("roomNumber", currentFloor);
        query.findInBackground(new FindCallback<ParseObject>() {
            @Override
            public void done(List<ParseObject> parseObjects, ParseException e) {
                if (e == null) {
                    Set<String> Rooms = new TreeSet<String>();
                    for (ParseObject object : parseObjects) {
                        String rooms = object.getString("roomNumber");
                        String room = "Room " + rooms;
                        Rooms.add(room);

                    }
                    addRoomstoList(Rooms);
                } else {
                    // Failed result
                    Toast.makeText(RoomActivity.this,
                            "Failed to load rooms. Make sure you are connected to a network",
                            Toast.LENGTH_LONG).show();
                }
            }
        });

    }

    private void addRoomstoList(Set<String> Rooms) {
        rooms.clear();
        for (String s : Rooms) {
            rooms.add(s);
        }
        roomAdapter.notifyDataSetChanged();
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
            getRooms();
        }




        return super.onOptionsItemSelected(item);
    }


}
