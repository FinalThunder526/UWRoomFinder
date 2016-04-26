//
//  StudyRoomTableView.swift
//  UW Room Finder
//
//  Created by Rajas Agashe on 12/18/14.
//  Copyright (c) 2014 Rajas Agashe. All rights reserved.
//

import UIKit

class StudyRoomTableView: UITableViewController {
    
    var floorName:String = String()
    var buildingName:String = String()
    var roomsArray:NSArray = NSArray()
    var roomsStringArray:NSMutableArray = NSMutableArray()
    var roomsDictionary:NSMutableDictionary = NSMutableDictionary()
    
    required init(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
    }
    
    override func viewDidLoad() {
        self.title = "Pick Room"
        println("floor name is " + floorName)
        

        var query = PFQuery(className: "StudyRoom")
        query.whereKey("buildingName", containsString: buildingName)
        query.whereKey("roomNumber", containsString: floorName)
        query.findObjectsInBackgroundWithBlock{
            (objects:[AnyObject]!, error:NSError!) -> Void in
            if error == nil{
                self.roomsArray = objects
                
            }else{
                println("Error was %@ %@", error, error.userInfo!)
            }
            self.tableView.reloadData()
            
        }
        self.tableView.reloadData()
    }
    
    override func prepareForSegue(segue: UIStoryboardSegue, sender: AnyObject?) {
        
        let vc = segue.destinationViewController as StudyRoomDetailViewController
        let index = tableView.indexPathForSelectedRow()?.row
        vc.descriptionOfRoom = (roomsArray.objectAtIndex(index!))["description"] as String
        vc.studyRoomsArray = self.roomsArray
        vc.roomsIndex = NSNumber(integer: index!)
        
    }
    
    
    override func tableView(tableView: UITableView, cellForRowAtIndexPath indexPath: NSIndexPath) -> UITableViewCell {
        
        let cell:StudyRoomCell = tableView.dequeueReusableCellWithIdentifier("cell", forIndexPath: indexPath) as StudyRoomCell
        //tableView.registerClass(StudyRoomCell.self, forCellReuseIdentifier: "cell")

        /*
        if(cell == nil){
            cell = StudyRoomCell(style: UITableViewCellStyle.Default, reuseIdentifier: "cell")
            //tableView.registerClass(StudyRoomCell.classForCoder(), forCellReuseIdentifier: "cell")

        }
        */
        /*
        var keySet = self.roomsDictionary.allKeys as NSArray
        var key = keySet[indexPath.row] as String
        var object = roomsDictionary.objectForKey(key) as PFObject
        var roomName:String = (object["roomNumber"]) as String
        var date:NSDate? = object["occupiedTill"] as? NSDate //["occupiedTill"] as? NSDate
        */
        var date:NSDate? = (roomsArray.objectAtIndex(indexPath.row))["occupiedTill"] as? NSDate
        //better date formatting
        var timeString = ""
        if(date != nil){
            let calendar = NSCalendar.currentCalendar()
            let components = calendar.components(.HourCalendarUnit | .MinuteCalendarUnit, fromDate: date!)
            let hour = components.hour
            let minutes = components.minute
            timeString = String(format: "%d:%d", hour, minutes)
        }
        //(roomsArray.objectAtIndex(indexPath.row))["roomNumber"] as String
        
        //cell?.textLabel.text = (roomsArray.objectAtIndex(indexPath.row))["roomNumber"] as? String
        //cell!.titleLabel = UILabel()
        
        var tempstring = (roomsArray.objectAtIndex(indexPath.row))["roomNumber"] as? String
        //cell.setCell(tempstring!)
        cell.titleLabel.text = (roomsArray.objectAtIndex(indexPath.row))["roomNumber"] as? String//roomName//
        cell.descriptionLabel.text = (roomsArray.objectAtIndex(indexPath.row))["description"] as? String
        //(roomsStringArray.objectAtIndex(indexPath.row) as String)
        //cell!.descriptionLabel.text = (roomsArray.objectAtIndex(indexPath.row))["description"] as? String
        //cell.setCell(tempstring!)
        println(date?.description)
        //if date != nil{
            cell.occupiedTillLabel.text = timeString //date?.description
        //}
        return cell
    }
    override func tableView(tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return roomsArray.count//roomsStringArray.count
    }
    
}
