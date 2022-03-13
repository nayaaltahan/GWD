
->AtTheSign
VAR player = "Robot"
===AtTheSign===
An ancient robot and a frog in overalls are helping each other get through the forest.
The two travelers walk by an old, wooden road sign.

*"THIS IS THE VILLAGE"  
~player = "Frog"
    **"No?"
    ~player = "Robot"
        ***"THE WRITING SAYS SO"
        ~player = "Frog"
            ****"That's a sign, genius."
            ****"You can read Critter?"
            ~player = "Robot"
                *****"I CAN READ EVERYTHING"
                *****["YES."]
                    "YES. CRITTER, ANCIENT, BUZZ AND EARTHTONGUE
        ***"DISAPPOINTING."
    **["That's a sign."]
        "That's a sign. The village is miles away still"
-
->DONE
            
===Hill===
There's a hill the robot cannot climb.

*"PATH BLOCKED"
    **["Jump, stupid!"]
    "Jump, stupid! Did you hit your head or something?"
        ***"YES."
        Robot fails to get up.
            ****["Embarassing..."]
                "Embarassing..."
            ****["Oh, you're really stuck..."]
                "Oh, you're really stuck. I'll get you up, give me a second."
            ****Say nothing.
            
        ***"I HIT EVERYTHING."
            Robot fails to get up.
            ****["Oh, you're really stuck..."]
                "Oh, you're really stuck. I'll get you up, give me a second."
            ****["Embarassing..."]
                "Embarassing..."
            ****Say nothing.
    **["I'll get you up."]
        I'll get you up - give me a second.
    **Say nothing
        
*Say nothing.
-
->DONE

===RobotGetsUp===
Robot gets up.

*"Finally!"
*["There you go!"]
    "There you go!"
-
    *"I AM VICTORIOUS"
    *"I CANNOT BE STOPPED"
-
*["Can you get me up there?"]
"Hey, big guy - think you can get me up there?"
    **"MY POWER IS LIMITLESS"
        ***"Wow. The confidence."
    **["YES"]
        ***Nice!
    **"NO."
        ***"Don't be a downer. At least try."
*Say nothing.
-
*["That's the statue of 'The Common Critter'"]
    "That's the statue of 'The Common Critter'. The Village folk are all about worshipping mediocrity."
    **["HIS BONES ARE METAL."]
     "HIS BONES ARE METAL."
        ***["Yes. What a waste."]
            "Yes. What a waste. Good, rust-free stuff too."
        ***"Duh."
                            ****"POSSIBLY"
                            ****Say nothing
        
    **"NORMALITY IS GOOD. "
        ***"Wow. You'll fit right in."
            ****"THANK YOU"
        ***"Not in the slightest"
-
->DONE

