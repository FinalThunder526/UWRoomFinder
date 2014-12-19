//
//  BuildingsTableView.swift
//  UW Room Finder
//
//  Created by Rajas Agashe on 12/18/14.
//  Copyright (c) 2014 Rajas Agashe. All rights reserved.
//

import UIKit

class BuildingsTableView: UITableViewController {

    override func viewDidLoad() {
        self.title = "Buildings"
    }
    override func tableView(tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return 1
    }
    
    override func tableView(tableView: UITableView, cellForRowAtIndexPath indexPath: NSIndexPath) -> UITableViewCell {
        var cell:UITableViewCell? = tableView.dequeueReusableCellWithIdentifier("cellIdentifier", forIndexPath: indexPath) as? UITableViewCell
        if(cell == nil){
            cell = UITableViewCell(style: UITableViewCellStyle.Value1, reuseIdentifier: "cellIdentifier")
        }
        
        cell!.textLabel.text = "happy"
        return cell!
    }
    
}
