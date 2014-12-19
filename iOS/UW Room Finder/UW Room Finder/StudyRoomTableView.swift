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
    var roomsArray:NSMutableArray = NSMutableArray()
    var roomsStringArray:NSMutableArray = NSMutableArray()
    
    override func viewDidLoad() {
        self.title = "Pick Room"
        for(var i = 0; i < roomsArray.count; i++){
            var roomName:String = (roomsArray.objectAtIndex(i))["roomNumber"] as String
            //roomName = (roomName as NSString).substringToIndex(1)
            if(!roomsStringArray.containsObject(roomName)){
                roomsStringArray.addObject(roomName)
            }
            
        }
    }
    
    override func tableView(tableView: UITableView, cellForRowAtIndexPath indexPath: NSIndexPath) -> UITableViewCell {
        var cell:UITableViewCell? = tableView.dequeueReusableCellWithIdentifier("cellIdentifier", forIndexPath: indexPath) as? UITableViewCell
        if(cell == nil){
            cell = UITableViewCell(style: UITableViewCellStyle.Value1, reuseIdentifier: "cellIdentifier")
        }
        
        cell!.textLabel.text = (roomsStringArray.objectAtIndex(indexPath.row) as String)
        return cell!
    }
    override func tableView(tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return roomsStringArray.count
    }
}
