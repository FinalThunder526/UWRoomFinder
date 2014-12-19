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
    var floorStringForSegue:String = String()
    var tempArrayForSegue:NSMutableArray = NSMutableArray()
    
    override func viewDidLoad() {
        self.title = buildingName + " Floors"
        println("happy")
        print(floorsArray.count)
        //tempArrayForSegue = floorsArray.mutableCopy() as NSMutableArray
        for(var i = 0; i < floorsArray.count; i++){
            var floorName:String = (floorsArray.objectAtIndex(i))["roomNumber"] as String
            floorName = (floorName as NSString).substringToIndex(1)
            if(!floorsStringArray.containsObject(floorName)){
                floorsStringArray.addObject(floorName)
                tempArrayForSegue.addObject(floorsArray.objectAtIndex(i))
            }
            
        }
        // create a floors array now from current model
        
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
    override func tableView(tableView: UITableView, didSelectRowAtIndexPath indexPath: NSIndexPath) {
        floorStringForSegue = floorsStringArray.objectAtIndex(indexPath.row) as String
    }
    
    //FIX THIS PASSED ARRAY FOR SURE!!!!!!!!!!!!!!!!!!!!
    override func prepareForSegue(segue: UIStoryboardSegue, sender: AnyObject?) {
        let vc = segue.destinationViewController as StudyRoomTableView
        vc.floorName = floorStringForSegue
        vc.roomsArray = tempArrayForSegue
    }
}
