//
//  BuildingsTableView.swift
//  UW Room Finder
//
//  Created by Rajas Agashe on 12/18/14.
//  Copyright (c) 2014 Rajas Agashe. All rights reserved.
//

import UIKit

class BuildingsTableView: UITableViewController {

    var buildingNames:NSMutableArray = NSMutableArray()
    
    override func viewDidLoad() {
        self.title = "Buildings"
        let building = PFObject(className:"StudyRoom")
        building["buildingName"] = "Lander"
        building["description"] = "In da Club"
        building.save()
        var query = PFQuery(className: "StudyRoom")
        var array:NSArray = query.findObjects() as NSArray
        for(var i = 0; i < array.count; i++){
            var name:String = (array.objectAtIndex(i))["buildingName"] as String
            buildingNames.addObject(name)
        }
        /*
        var gameScore = PFObject(className:"GameScore")
        gameScore["score"] = 1337
        gameScore["playerName"] = "Sean Plott"
        gameScore["cheatMode"] = false
        gameScore.saveInBackground()
        */
    }
    override func tableView(tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return buildingNames.count
    }
    
    override func tableView(tableView: UITableView, cellForRowAtIndexPath indexPath: NSIndexPath) -> UITableViewCell {
        var cell:UITableViewCell? = tableView.dequeueReusableCellWithIdentifier("cellIdentifier", forIndexPath: indexPath) as? UITableViewCell
        if(cell == nil){
            cell = UITableViewCell(style: UITableViewCellStyle.Value1, reuseIdentifier: "cellIdentifier")
        }
        
        cell!.textLabel.text = buildingNames.objectAtIndex(indexPath.row) as String
        return cell!
    }
    
}
