//
//  StudyRoomDetailViewController.swift
//  UW Room Finder
//
//  Created by Rajas Agashe on 12/22/14.
//  Copyright (c) 2014 Rajas Agashe. All rights reserved.
//

import UIKit

class StudyRoomDetailViewController: UIViewController, UITextFieldDelegate {
    
    
    @IBOutlet var studyRoomDescription: UILabel! = UILabel()
    @IBOutlet var timeSelected: UILabel! = UILabel()
    @IBOutlet var usernameTextField: UITextField! = UITextField()
    var descriptionOfRoom:String = String()
    var timeToReserve = 0
    var studyRoomsArray:NSArray = NSArray()
    //var roomPFObject:PFObject
    var roomsIndex:NSNumber = NSNumber()
    
    
    @IBAction func subtractTime(sender: AnyObject) {
        timeToReserve -= 15
        timeSelected.text = String(timeToReserve)
    }
    
    @IBAction func addTime(sender: AnyObject) {
        timeToReserve += 15
        timeSelected.text = String(timeToReserve)
    }
    
    @IBAction func checkIn(sender: AnyObject) {
        //first need to create accurate date based on current time
        /*
        NSDate *localDate = [NSDate date]; //this will have 7:30AM PDT
        NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
        dateFormatter.dateFormat = @"yyyy-MM-dd ";
        [dateFormatter setTimeZone:[NSTimeZone timeZoneWithName:@"EDT"]];
        NSString *convertedTimeString = [dateFormatter stringFromDate:localDate];
        */
        var currentDate = NSDate()
        var dateFormatter:NSDateFormatter = NSDateFormatter()
        dateFormatter.dateFormat = "yyyy-MM-dd HH:mm:ss" as String
        dateFormatter.timeZone = NSTimeZone(abbreviation: "PST")
        let currentDateString  = (dateFormatter.stringFromDate(currentDate)) as String
        print(currentDateString)
        
        //let update
        //let updatedDateString = (currentDateString as NSString).substringToIndex(<#to: Int#>)
        
        var val = roomsIndex.integerValue

        let object = (studyRoomsArray.objectAtIndex(val)) as PFObject
        object["occupant"] = usernameTextField.text as String
        
        var interval = Double(timeToReserve * 60)
        //println("interval is: %.f",  interval)
        var date = NSDate(timeInterval: interval, sinceDate: NSDate())
        object["occupiedTill"] = dateFormatter.dateFromString(currentDateString)
        println("current date " + NSDate().description)
        println("updated date " + date.description)
        
        object.save() // change to in background with block
        
    }
    func textFieldShouldReturn(textField: UITextField) -> Bool {
        usernameTextField.resignFirstResponder()
        return true
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.title = studyRoomsArray.objectAtIndex((roomsIndex.integerValue))["roomNumber"] as? String
        // Do any additional setup after loading the view.
        println(description)
        var val = roomsIndex.integerValue
        studyRoomDescription.text = studyRoomsArray.objectAtIndex(val)["description"] as? String
        usernameTextField.delegate = self
        /* working date method
        var currentDate = NSDate()
        var dateFormatter:NSDateFormatter = NSDateFormatter()
        dateFormatter.dateFormat = "yyyy-MM-dd HH:mm:ss" as String
        dateFormatter.timeZone = NSTimeZone(abbreviation: "PST")
        print(dateFormatter.stringFromDate(currentDate))
        println(dateFormatter.stringFromDate(currentDate))
        */

    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    override func touchesBegan(touches: NSSet, withEvent event: UIEvent) {
        self.usernameTextField.resignFirstResponder()
    }

    /*
    // MARK: - Navigation

    // In a storyboard-based application, you will often want to do a little preparation before navigation
    override func prepareForSegue(segue: UIStoryboardSegue, sender: AnyObject?) {
        // Get the new view controller using segue.destinationViewController.
        // Pass the selected object to the new view controller.
    }
    */

}
