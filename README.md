# Mono8 

- **A CHIP-8 emulator written with C# and MonoGame!**
- **By Erievs** 

## About

    - Mono8 is a very simple emulator for CHIP-8. 
    
    - This emulator is more intended as a fun side project  

    ## Issues

        - Input is wonky, I think they aren't really the proper keys
          that were in the orginal CHIP-8, I am sorry. I may try to
          improve I mean you can play pong alr. Maybe someone can fix it.

        - Hard coded "ROM" path, I mean it should be pretty easy to fix it
          and I may one day.
      
    ## Support

    ### CHIP-8

        - Passes the flags test.

        - Plays pong fine!

    ### SUPER-CHIP

        - Not supported
    
    ### XO-CHIP

        - Not Supported
    
    ## Design

    - I designed this to be (hopefully) fairly easy to extend support for new system.
    
    - There is a base 'System' class which we have a instance of in Game1, depending
      on the file extension, we set the reference to system to whatever System you like. So if it is a .ch8 for example, we set system to equal System8! I did this so you don't have to worry as much about breaking comp or something. I
      mean this is still pretty eh programming but it works. 

    - Scroll8 just deals with scrolling, it was used for SUPER-CHIP/XO-CHIP support,
      but I removed them from this GitHub build it isn't ready.
    
