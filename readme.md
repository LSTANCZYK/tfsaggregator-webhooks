![Build badge](https://tfsaggregator.visualstudio.com/_apis/public/build/definitions/2e747373-c780-4b2c-823d-98a3fd2b4e99/9/badge)

This is an alternative version of TFS Aggregator that supports [Visual Studio Team Services (VSTS)](https://www.visualstudio.com/team-services/).
It can be used on premises also, starting from TFS 2015: you have to provide the hosting environment.
You can reuse the existing Rules with minimal changes.

Try it now 
[![Deploy to Azure](http://azuredeploy.net/deploybutton.png)](https://azuredeploy.net/)

> **Note** The above button does not work on branches with a slash (`/`) in name e.g. `release/2.3`.

## Example Uses

 - Update the state of a Bug, PBI (or any parent) to "In Progress" when a child gets moved to "In Progress"
 - Update the state of a Bug, PBI (or any parent) to "Done" when all children get moved to "Done" or "Removed"
 - Update the "Work Remaining" on a Bug, PBI, etc with the sum of all the Task's "Work Remaining".
 - Update the "Work Remaining" on a Sprint with the sum of all the "Work Remaining" of its grandchildren (i.e. tasks of the PBIs and Bugs in the Sprint).
 - Sum up totals on a single work item (i.e. Dev Estimate + Test Estimate = Total Estimate)
 - Create new work items
 - Create new work item links

# Documentation

The complete documentation is available on the [project's Documentation Site](https://tfsaggregator.github.io/).

# Contributing to the Project

Please read the [Contributing](CONTRIBUTING.md) document.
