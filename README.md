# SG Radio for Windows 8.1
Windows 8.1 "Universal" App for Singapore Radio stations

![Logo](https://raw.githubusercontent.com/ReignOfComputer/SG-Radio-for-Windows-8.1/master/SGRadioLogo.png)
![Screenshot](https://raw.githubusercontent.com/ReignOfComputer/SG-Radio-for-Windows-8.1/master/SGRadioSS.png)

Back in 2012, I was actively developing Windows Phone and Windows 8's Metro/Modern/Universal apps - which eventually led to a stint at Microsoft. SG Radio was one such app, and was developed and released as a launch app for Windows 8. SG Radio then went through a drastic upgrade in 2013 (for the release of Windows 8.1), and used as a showcase app in many of Microsoft's demos for both consumers and developers.

SG Radio aggregates various Singapore Radio stations, including Internet radio stations. It's able to get the title of the track that's currently playing and in the past, used Mediacorp's now defunct API to retrieve lyrics as well.

There are various other features built in, such as a standby mode that shows the current time and song, and a timer to shutdown playback after some time. Users are able to mark stations as a favorite, and can even bookmark a song that's currently playing.

It took advantage of various Windows 8 features, like Live Tiles, Background Tasks, Media Controls, Charms, and AppBar to name a few.

Back then, the various stations used a plethora of streaming technologies - the usual audio/mpeg or audio/mp3, and more complex ones like HLS and PLS playlists. As such, libraries from phonesm were used (IIRC) and it was super complicated to bring everything together. As of 2018, all the streams appear to be natively supported by Windows and this project just got a whole lot smaller.

This Windows Store version has been downloaded 44,422 times as of 19th February 2018. The Windows Phone app is maintained as a separate solution.

I'm making the source code public because I have no plans to continue development at this time (past the upcoming maintenance to fix a lot of broken stuff after 3 years). I'd love to upgrade this and target Windows 10, but I think I'm too far out of the apps scene to figure it out.

Most of the code you'll see here is crap, so I apologize in advance. This was developed at a time where I was just starting out as a developer, and helped me learn a lot.

-----

v4.0.0.3 - 21 June 2018
- FIXED: SPH migrated some of their streams, thanks everyone who reported.

v4.0.0.2 - 7th March 2018
- CHANGED: Microsoft apparently changed their policies (probably in a bid to piss off more developers and get them off their platform), so here's a quick change to appease them regarding donation links

v4.0.0.1 - 21st February 2018
- HOTFIX: Users with versions before V4 would face issues with favorites and starred tracks after updating - this update wipes the database to fix that

v4.0.0.0 - 19th February 2018
- Whoa, it's been over 3 years since the last update! Were you an active user of SG Radio? Let me know how I did by rating the app!
- FIXED: All the streams that have been migrated
- FIXED: All the rebranding for the stations
- ADDED: Various new stations
- ADDED: Settings Charms button in AppBar for Windows 10 users
- CHANGED: Optimized how streaming works, much less lag now
- CHANGED: Song name now appears before the artist instead of the other way around
- CHANGED: Current playing station and track are now neatly centered
- CHANGED: Removed redundant Refresh button
- WARNING: All favorites and pinned tiles need to be reset after this major update

v3.4.0.5 - 9th February 2015
- FIXED: Pesky stations. Fixed Kiss 92 stream migration

v3.4.0.0 - 16th January 2015
- FIXED: Hot FM 91.3, BBC World Service and UFM 100.3 streams have been updated
- REMOVED: The Live Radio has been removed as their stream is down

v3.3.0.0 - 15th June 2014
- FIXED: Playing BBC World Service would cause the app to crash
- UPDATED: Mediacorp Streams are now better handled
- UPDATED: General performance and code optimization

v3.2.0.0 - 15th December 2013
- FIXED: Live Tiles wouldn't clear after app closes
- FIXED: Primary Tile would show past songs
- FIXED: Some lyrics were malformed or contained trailing data
- FIXED: The Live Radio current song data was incorrectly cached
- FIXED: Starred Tracks were not displaying properly for some stations
- CHANGED: Search Box and Status behavior in snap mode
- CHANGED: Standby View behavior in snap mode
- CHANGED: Touched up some graphics
- ADDED: Power 98 FM
- ADDED: 88.3 Jia FM
- NEW: Enhanced snap view for tablets and maximum snaps
- NEW: Forceful Live Tile clear via Background Task
- NEW: Hovering over or pressing current track shows full title
- UPDATE: Future-proofed more obsolete Windows 8 code
