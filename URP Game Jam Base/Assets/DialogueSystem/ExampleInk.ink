-> start
=== start
This is an example of an ink script!
* [Can I only pick this option once?] 
-> yesOnlyOnce
+ [What about this one?] 
-> asMany
+ Will this one print to the screen? 
-> echo
+ [Tell me about branching dialogue?] 
-> branchStart
+ [Tell me about items?] 
-> itemStart
+ [Tell me about skills?] 
-> getSkill1
+ITM exampleItem 
-> useItemExample
+SK21 [Throw hands using SkillTwo!] 
-> engarde
+ [Okay, I'm done with this.] -> END


=== yesOnlyOnce
Yes, only once. It has an * beside it in Ink.
-> start

=== asMany
As many times as you like, it has a + sign beside it, afterall.
-> start

=== echo
... yes. It's weird when you do that. Don't put words in my mouth.
-> start


=== branchStart
Okay, how about this? 
-> woods


=== woods
You are in the woods.
+ [Go Left] 
-> woodsleft
+ [Go Right] 
-> woodsright

=== woodsleft
You walk into a clearing and see a deer, what do you do?
+ [Stay very still so I don't frighten it.] 
-> still
+ [Befriend the deer.] 
-> friendship
+ [Shoot it.] 
-> hunt


=== still
You try your hardest not to move a muscle.
Sweat runs down your temples. The deer does not move.
Hours pass.
Then days.
Weeks.
You are awarded the Guiness Book of World Records record for standing petrified still.
The deer snubs your invitation to the award ceremony, jealous and full of spite.
-> start


=== friendship
You approach the deer slowly, and give it a gentle pet...
To your surprise, it does not run, nor cower.
Its eyes show no sign of fear.
You are one with the forest, as the forest is to the deer.
-> start


=== hunt
You shoot - and miss!
The deer sprints into the woods.
You feel the knife of guilt in your stomach.
-> start


=== woodsright
You think you are so clever.
+ [Go right again.] 
-> rightAgain
+ [Go left?] 
-> loop

=== loop
You wander in circles for what feels like hours.
-> woodsright

=== rightAgain
You notice the sun is beginning to set.
You wonder how long it will be until you are found.
+ [Go left.] 
-> woods
+ [Go right.] 
-> loop


=== itemStart
Would you like to try using an item?
Here, I'll give you an example.
ADD exampleItem
-> start


=== useItemExample
Alright, that'll be $32,000.
Oh, you can't pay? I'll take that example item of yours then!
RMV exampleItem
-> start


=== getSkill1
SK1+
-> skillStart


=== skillStart
Okay, what would you like to know?
You can only respond with options you meet the skill requirement for.
+SK11 [I have SkillOne! Can I use it to increase SkillTwo?]
-> skillOne
+SK21 [Whoa I have SkillTwo now?? Rad!] 
-> skillTwo
+SK31 [This one won't show up, cause I don't have the skill points for it :(]
-> howDid
+ [Okay, that's enough.] 
-> start


=== skillOne
Sure! How about I take your skillOne and give you some SkillTwo?
SK1-
SK2+
-> skillStart

=== skillTwo
Yeah, cool aye? How about you use that back at the start?
-> start

=== howDid
Wh- how did you do that?
You must be a wizard.
I'm deathly allergic to wizards..
END
-> END

=== engarde
Engarde!
SK2-
Nice try, but you are no match for my swordsplay!
-> start

