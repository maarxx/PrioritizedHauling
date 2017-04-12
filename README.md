# PrioritizedHauling

This is a mod for the game RimWorld by Ludeon Studios.

It allows you to specify certain types of objects to be hauled at higher priority, in a customizable order.

# Table of Contents

* [Introduction and Explanation](#introduction-and-explanation)
* [Specific Additional Features](#specific-additional-features)
* [How to Install](#how-to-install)
* [How to Update](#how-to-update)
* [Bugs, New Features, and Updates](#bugs-new-features-and-updates)

# Introduction and Explanation

[![](http://i.imgur.com/9L4f8u7.png)](https://ludeon.com/forums/index.php?topic=28066.0) **IMPORTANT: This mod requires HugsLibs to be loaded before it.**

You'll add the mod. You'll enable the mod.

If you care, you'll notice it adds a new WorkGiver. That might not mean anything to you. That's okay. But if you are using something like Fluffy's WorkTab in Detailed Mode, you can see the new WorkGiver:"Haul priority things" inside WorkType:"Haul".

By default, this WorkGiver won't do anything.

Within the game, the mod adds a MainTab, probably in the far-bottom-right-corner, labeled "PrioritizedHauling", with buttons for control functions.

You can use these buttons to set certain items ("ThingDefs") or certain categories of item ("ThingCategoryDef") for prioritized hauling, and then all of these items will be hauled first, **in the priority order you've specified**, using the new WorkGiver.

Click "Add Priority ThingDef" or "Add Priority ThingCategoryDef" to view the list of items/categories available for hauling that are not set for priority, and click an entry to add it for priority.

Click "Increment/Decrement Priority" to view the existing priority list, in order, and click an entry to bump it higher or lower in the list, accordingly.

Click "Remove Priority" to view the existing priority list, in order, and click an entry to remove it from priority.

Click "Clear Priority" to completely clear the priority list.

When there is nothing left to haul on priority using this new WorkGiver, Pawns will fail over to the vanilla hauling WorkGiver for any remaining items.

**Technical Compatibility Note:**

This mod uses HugsLib to detour one method: `JobGiver_Work.TryGiveJob(Pawn pawn)`

This mod is therefore **incompatible** with any other mod which attempts to detour the same method.

# Specific Additional Features

If you've built a very complex list of things to be hauled in a very specific order, you can Export the list and then Import it later.

If you've grown attached to your list, be sure to Export it before quitting the game, because otherwise it is not saved. And be sure to Import it when loading back into the game.

You can even use this Exports/Imports across different colonies, they are universal.

# How to Install

At the top of this page, on the right-hand-side, a little ways down, will be a green button, labeled "Clone or download". Click it, then click "Download ZIP". Your browser will download it.

Unzip it, and it will spew out a single folder, which is probably named something like `PrioritizedHauling-master`.

Assuming you are working with default installation directories on a Windows system, you will want to move this entire folder into:

`C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods`

If you did it correctly, the result should be a directory structure that looks something like this:

`C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\PrioritizedHauling-master\Assemblies`

Then restart RimWorld and enable it like any other mod.

# How to Update

First and foremost, please note that I never test updating mods on older saved games. You can try it, but please assume that a new game might always be necessary.

I also don't explicitly test whether the mod can be disabled on an existing game. Please also assume that a new game might always be necessary.

With that out of the way:

Updating is just deleting the previous version of the mod and then installing a new version.

So again, assuming default installation directories on a Windows system, you'll want to delete the same folder that you added during installation, which probably looks something like:

`C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\PrioritizedHauling-master`

Then follow the previous instructions to download and install the new version, by repeating the same steps as installing the original version.

# Bugs, New Features, and Updates

You are currently looking at a GitHub repository for managing application code. I work out of this GitHub repository, and so to talk about bugs, new features, or updates, you need to know a little bit about navigating a GitHub repository like this one.

Beneath the aforementioned green button "Clone or download", it will say "Latest commit", followed by a couple random characters, followed by an amount of time. This stamp indicates how long ago the mod was last updated.

So if you think you found a problem, check this stamp. Perhaps the mod has already been updated since you downloaded it last, and you should download a new version and update. See the above instructions for how to update.

By default, you are probably looking at the "master" branch. You can see this at the top of the page, on the left-hand-side, a little ways down, it will say "Branch: something", probably "Branch: master", with a little down arrow.

The "master" branch contains the current version of the mod which I consider to be tested and stable. Mostly. I guess.

Most (but not all) of my mods have a "beta" branch for pre-release, which might offer new features or bug fixes that should probably work, theoretically, but I haven't really done much testing on, so I'm not quite sure.

So if you tried updating from the "master" branch, and you still think you found a bug or a problem, or if you just want to try the shiny new features before everybody else, consider downloading the "beta" branch and installing that instead.

To do this, just click the button where it says "Branch: master", and then click the option for "beta". Congratulations! You've changed branches! Follow the same steps to download and install, except instead of `PrioritizedHauling-master` it will now be `PrioritizedHauling-beta`. You can have both versions installed, but please don't try to have both versions enabled at once using the in-game Mod menu.

You will probably see other choices besides "master" and "beta", but I don't recommend clicking them. I am probably in the middle of working on them, and they are probably only halfway done, and broken, otherwise they'd already be part of "beta".

So if you tried updating the master branch, and you tried the beta branch, and you still thing you found a bug, or a problem, or want to suggest a new feature, wander over to the "Issues" tab. You can find this at the very top of the page, you are currently on the first tab "Code", you want to change to the second tab "Issues".

You can look here to see if your bug, issue, or suggestion is already present, and add comments if you wish.

If it's not there, look to the right-hand-side, click the green button "New issue", just type a Title, and Leave a comment, and then look below and click the green button "Submit new issue". I will get back to you. Maybe. Eventually. Meanwhile, other users might be able to chime in and help you too!
