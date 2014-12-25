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
    var allObjects:NSArray = NSArray()
    //var buildingNameForSegue:String = String()
    //var buildingNamesSet:NSMutableSet = NSMutableSet()
    
    override func viewDidLoad() {
        self.title = "Buildings"
        
        buildingNames.addObject("Lander")
        buildingNames.addObject("Alder")
        buildingNames.addObject("Elm")
        //buildingNames.addObject("Elm") UNCOMMENT THIS
        var query2 = PFQuery(className: "StudyRoom")
        var names:[String] = ["Lander", "Alder", "Elm"] //UNCOMMENT THIS, "Elm"]
        query2.whereKey("buildingName", notContainedIn: names)
        query2.findObjectsInBackgroundWithBlock {
            (results: [AnyObject]!, error: NSError!) -> Void in
            if error == nil {
                // results will contain users with a hometown team with a winning record
                for(var i = 0; i < results.count; i++){
                    println(results[i]["buildingName"] as String)
                    if(!(self.buildingNames.containsObject(results[i]["buildingName"] as String))){
                        self.buildingNames.addObject((results[i]["buildingName"] as String))
                    }
                }
                self.tableView.reloadData()
            }
        }
    }
    
    // only call this function if you need to repopulate some other floow
    func populatePoplarRooms(){
        
    }
    func populateElmRooms(){
        //let studyRoom = PFObject(className:"StudyRoom")
        for(var i = 4; i < 8; i++){
            for(var j = 0; j < 3; j++){
                let studyRoom = PFObject(className:"StudyRoom")
                studyRoom["buildingName"] = "Elm"
                var tempRoomNumber:String = String(i) + "02"
                tempRoomNumber.append(Character(UnicodeScalar(66+j)))
                //string.append(Character(UnicodeScalar(50)))
                studyRoom["roomNumber"] = tempRoomNumber as String
                if(j == 0){
                    studyRoom["description"] = "Small"
                }else if j == 1{
                    studyRoom["description"] = "Big"
                }else if j == 2{
                    studyRoom["description"] = "Big"
                }else{
                    studyRoom["description"] = "This study room contains a lot of whiteboard space as well as many tables and sofas"
                }
                studyRoom.save()
                
            }
            
        }
    }
    func populateLanderRooms(){
        //let studyRoom = PFObject(className:"StudyRoom")
        for(var i = 3; i < 9; i++){
            for(var j = 0; j < 4; j++){
                let studyRoom = PFObject(className:"StudyRoom")
                studyRoom["buildingName"] = "Lander"
                var tempRoomNumber:String = String(i) + "02"
                tempRoomNumber.append(Character(UnicodeScalar(65+j)))
                //string.append(Character(UnicodeScalar(50)))
                studyRoom["roomNumber"] = tempRoomNumber as String
                if(j == 0){
                    studyRoom["description"] = "This study room contains a whiteboard and 6 chairs"
                }else if j == 1{
                    studyRoom["description"] = "This study room contains a TV, kitchen as well as tables and sofas"
                }else if j == 2{
                    studyRoom["description"] = "This study room contains a TV, kitchen as well as tables and sofas"
                }else{
                    studyRoom["description"] = "This study room contains a lot of whiteboard space as well as many tables and sofas"
                }
                studyRoom.save()
                
            }
            
        }
    }
    func populateAlderRooms(){
        //let studyRoom = PFObject(className:"StudyRoom")
        for(var i = 3; i < 8; i++){
            for(var j = 0; j < 4; j++){
                let studyRoom = PFObject(className:"StudyRoom")
                studyRoom["buildingName"] = "Alder"
                var tempRoomNumber:String = String(i) + "02"
                //tempRoomNumber.append(Character(UnicodeScalar(65+j)))
                //string.append(Character(UnicodeScalar(50)))
                studyRoom["roomNumber"] = tempRoomNumber as String
                if(j == 0){
                    studyRoom["description"] = "Big"
                    studyRoom["roomNumber"] = String(i) + "02" + "B"
                }else if j == 1{
                    studyRoom["description"] = "Small"
                    studyRoom["roomNumber"] = String(i) + "02" + "C"
                }else if j == 2{
                    studyRoom["description"] = "Small"
                    studyRoom["roomNumber"] = String(i) + "02" + "D"
                    
                }else{
                    studyRoom["description"] = "Small"
                    studyRoom["roomNumber"] = String(i) + "02" + "F"
                }
                studyRoom.save()
                
            }
            
        }
    }
    
    override func tableView(tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return buildingNames.count
    }
    
    
    
    override func tableView(tableView: UITableView, cellForRowAtIndexPath indexPath: NSIndexPath) -> UITableViewCell {
        var cell:UITableViewCell? = tableView.dequeueReusableCellWithIdentifier("cellIdentifier", forIndexPath: indexPath) as? UITableViewCell
        if(cell == nil){
            cell = UITableViewCell(style: UITableViewCellStyle.Value1, reuseIdentifier: "cellIdentifier")
        }
        
        cell!.textLabel.text = (buildingNames.objectAtIndex(indexPath.row) as String)
        return cell!
    }
    
    
    
    override func prepareForSegue(segue: UIStoryboardSegue, sender: AnyObject?) {
        if(segue.identifier == "segue"){
            let controller = segue.destinationViewController as FloorsTableView
            let selectedRowVal = tableView.indexPathForSelectedRow()?.row
            controller.buildingName = buildingNames.objectAtIndex(selectedRowVal!) as String//buildingNameForSegue
            
        }
        
    }
    
}
