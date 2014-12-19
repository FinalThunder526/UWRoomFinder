package com.siddg.uwroomfinder;

import android.app.ProgressDialog;
import android.content.Intent;
import android.os.Build;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
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


public class MainActivity extends ActionBarActivity {

    private ListView buildingList;
    private List<String> buildings;
    private ArrayAdapter<String> buildingAdapter;
    private ProgressDialog loadingFeedback;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        // App entry
        buildingList = (ListView) findViewById(R.id.buildingList);
        buildingList.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                String buildingName = buildings.get(position);
                Intent i = new Intent(MainActivity.this, FloorActivity.class);
                i.putExtra("buildingName", buildingName);
                startActivity(i);

            }
        });
        buildings = new ArrayList<String>();
        buildingAdapter = new ArrayAdapter<String>(this, android.R.layout.simple_list_item_1, buildings);
        buildingList.setAdapter(buildingAdapter);
        getBuildings();


    }

    private void getBuildings() {
        ParseQuery<ParseObject> query = ParseQuery.getQuery("StudyRoom");
        loadingFeedback = ProgressDialog.show(this, "", "Loading UW Buildings");
        query.findInBackground(new FindCallback<ParseObject>() {
            @Override
            public void done(List<ParseObject> parseObjects, ParseException e) {
                if (e == null) {
                    Set<String> Buildings = new TreeSet<String>();
                    for (ParseObject object : parseObjects) {
                        String build = object.getString("buildingName");
                        Buildings.add(build);
                    }
                    addBuildingstoList(Buildings);
                } else {
                    // Failed result
                    Toast.makeText(MainActivity.this, "Failed to load building list", Toast.LENGTH_LONG).show();
                }
            }
        });
    }

    private void addBuildingstoList(Set<String> Buildings) {
        buildings.clear();
        for (String s : Buildings) {
            buildings.add(s);
        }
        buildingAdapter.notifyDataSetChanged();
        loadingFeedback.dismiss();
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        // Refresh action
        if (id == R.id.action_refresh) {
            getBuildings();
        }


        return super.onOptionsItemSelected(item);
    }
}
