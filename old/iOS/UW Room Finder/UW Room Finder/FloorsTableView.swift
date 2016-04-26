//
//  FloorsTableView.swift
//  UW Room Finder
//
//  Created by Rajas Agashe on 12/18/14.
//  Copyright (c) 2014 Rajas Agashe. All rights reserved.
//

import UIKit

class FloorsTableView: UITableViewController {

    var buildingName:String = String()
    var floorsArray:NSMutableArray = NSMutableArray()
    var floorsStringArray:NSMutableArray = NSMutableArray()
    var tempArrayForSegue:NSMutableArray = NSMutableArray()
    
    override func viewDidLoad() {
        self.title = buildingName + " Floors"
        print(floorsArray.count)
        
        // create a floors array now from current model
        var query = PFQuery(className: "StudyRoom")
        query.whereKey("buildingName", equalTo: buildingName)
        query.findObjectsInBackgroundWithBlock{
            (objects:[AnyObject]!, error:NSError!) -> Void in
            if(error == nil){
                for object in objects{
                    let roomNum = object["roomNumber"] as String
                    println(roomNum)
                    let floorName = (roomNum as NSString).substringToIndex(1)
                    if(!(self.floorsStringArray.containsObject(floorName))){
                        self.floorsStringArray.addObject(floorName)
                    }
                    
                    //self.floorsStringArray = self.floorsStringArray.sorted{ $0.localizedCaseInsensitiveCompare($1) == NSComparisonResult.OrderedAscending }
                }
                self.tableView.reloadData()
            }else{
                print("Error is %@ %@", error, error.userInfo!)
            }
        }
        
    }
    
    
    
    override func tableView(tableView: UITableView, cellForRowAtIndexPath indexPath: NSIndexPath) -> UITableViewCell {
        
        var cell:UITableViewCell? = tableView.dequeueReusableCellWithIdentifier("cellIdentifier", forIndexPath: indexPath) as? UITableViewCell
        if(cell == nil){
            cell = UITableViewCell(style: UITableViewCellStyle.Value1, reuseIdentifier: "cellIdentifier")
        }
        
        cell!.textLabel.text = (floorsStringArray.objectAtIndex(indexPath.row) as String)
        return cell!
    }
    override func tableView(tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return floorsStringArray.count
    }
    
    
    // fix the array
    override func prepareForSegue(segue: UIStoryboardSegue, sender: AnyObject?) {
        let vc = segue.destinationViewController as StudyRoomTableView
        let index = tableView.indexPathForSelectedRow()?.row
        let selectedFloorName = floorsStringArray.objectAtIndex(index!) as String
        vc.floorName = selectedFloorName
        vc.buildingName = self.buildingName
        
    }
}
