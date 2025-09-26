using System;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using GameManager.Enums;

namespace GameManager.System
{

    public class System8 : SystemBase
    {

        // CPU Registers & More
        private ushort opcode;

        private ushort[] memory = new ushort[4096]; // 4k; memory
        private ushort[] vRegs = new ushort[16]; // 15 CPU registers; +1 carry flag

        private ushort indexReg;
        private ushort programCounter; // Program counter which (can) have a value from 0x000 through 0xFFF.

        // The Stack!
        private ushort[] stack = new ushort[16];
        private ushort stackPointer;


        // Sound (Very Simple; Just 60hz and beeps and such)
        private uint delayTimer;
        private uint soundTimer;

        // Graphics
        private bool drawFlag = false;

        // Imports
        private static readonly Random random = new Random();

        private Screen8 _screen;
        private Sound8 _sound;
        private Keyboard8 _keyboard;


        public System8(Screen8 screen, Sound8 sound, Keyboard8 keyboard) : base(screen, sound, keyboard)
        {

            // Set
            _screen = screen;
            _sound = sound;
            _keyboard = keyboard;

            // Set System
            programCounter = 0x200; // Program ROM and work RAM
            opcode = 0;
            indexReg = 0;
            stackPointer = 0;


            // Set Mem
            // Loop throughout the memory and set.
            for (int i = 0; i < memory.Length; i++)
            {
                memory[i] = 0;
            }

            // Set Stack
            // Loop throught the vregs and set stack.
            for (int i = 0; i < 16; i++)
            {
                vRegs[i] = 0;
                stack[i] = 0;
            }

            // Load Font
            for (int i = 0; i < 80; i++)
            {
                memory[i] = Font.FONT[i];
            }

            // Misc
            drawFlag = false;
            delayTimer = 0;
            soundTimer = 0;

        }

        /// <summary>
        /// Method <c>LoadProgram</c> Loads rom's bytes into the memory.
        /// </summary>
        public override void LoadProgram(string path)
        {

            byte[] bytes = File.ReadAllBytes(path);

            bytes.CopyTo(memory, 0x200); // set the memory; we start at 0x200 (hex) which is 512, which is where roms should be loaded

            Debug.WriteLine("\n");
            for (int i = 0; i < memory.Length; i++)
            {
                Console.Write(" " + Utils.ToHex(memory[i]) + " ");
            }
            Debug.WriteLine("\n");


            Debug.WriteLine($"\nFirst Byte Of Rom: {Utils.ToHex(memory[512])}");

        }

        /// <summary>
        /// Method <c>SetOpcode</c> sets the opcode from memory using the programCounter.
        /// </summary>
        public void SetOpcode()
        {
            opcode = (ushort)(memory[programCounter] << 8 | memory[programCounter + 1]);
        }

        /// <summary>
        /// Method <c>CPUStep</c> preforms the functions of the CPU.
        /// </summary>
        public override void CPUStep()
        {
            ushort x = 0; // just a thing to keep track of some opcodes and stuff
            ushort y = 0; // not used as much I forgot to (I got confused at first and didn't use it in time still used a bit )
            byte carry = 0; // for carrying logic, it makes it easier/cleaner for setting vF and stuff

            Debug.WriteLine("System: " + $"opcode {Utils.ToHex(opcode)} : {programCounter} : {Utils.ToHex(opcode & 0xF000)} : {Utils.ToHex(opcode & 0xF00F)} : {Utils.ToHex(opcode & 0xF0FF)}");

            // Not Realtive To Rom
            switch (opcode)
            {

                case 0xD:

                    Debug.WriteLine("XOChip");

                    return;

                case 0x00E0:

                    _screen.Clear();

                    drawFlag = true;

                    programCounter += 2;

                    return;

                case 0x00EE:

                    // returns from a subroutine
                    programCounter = stack[stackPointer--];
                    drawFlag = true;

                    programCounter += 2;

                    return;
            }

            // Realtive To ROM
            // yeah I forgot that we have to do & 0xF000, and spent 2 and half hours trying to figure out why things were not jumping.
            // some other people use nn in hexes like 3XNN, we use k here, doesn't reall matter as it is just in the comments

            // >>> is unsigned right shift
            // >> is right shift

            // << left shift

            // & AND (result of & is be true if only the both values are true) (binary)

            // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/bitwise-and-shift-operators

            // may have to check underflow and overflow crap
            // the gulrak hammers checking it home (I think we do)

            // oh I also really hope casting ushorts and using ushorts wont cause issues (they use less memory, and I guess map closer? but prob dont matter in as basic as a system)
            // I guess we can use units at wrost (I wish there was unsigned bytes, they map closer)

            // vF is of course in the vReg, I just didn't feel like fully typing it out 
            // well I was going to but by the time I relaize I am like eh

            // VF is 0xF

            // Basic functions
            switch (opcode & 0xF000)
            {

                case 0x1000:

                    // 1NNN we just jump the address to NNN
                    programCounter = (ushort)(opcode & 0x0FFF);

                    return;

                case 0x2000:

                    // 2NNN calls a subroutine at NNN
                    stack[++stackPointer] = programCounter;  // set the stack and increment the point by 1 (++)

                    programCounter = (ushort)(opcode & 0x0FFF);

                    return;

                case 0x3000:

                    // 3XNN skip the next instruction if vRegsX is equal to kk (so 0x3kk in this case).
                    if (vRegs[(opcode & 0x0F00) >>> 8] == (opcode & 0x00FF))
                    {
                        programCounter += 4;
                    }
                    else
                    {
                        programCounter += 2;
                    }

                    return;

                case 0x4000:

                    // 4XNN skips next instruction if vRegsX is not equal to kk  (so 0x4kk in this case).
                    if (vRegs[(opcode & 0x0F00) >>> 8] != (opcode & 0x00FF))
                    {
                        programCounter += 4;
                    }
                    else
                    {
                        programCounter += 2;
                    }

                    return;

                case 0x5000:

                    // 5XY0 skips next instruction if vRegsX is equal to vRegsY.
                    if (vRegs[(opcode & 0x0F00) >>> 8] == vRegs[(opcode & 0x00F0) >>> 4])
                    {
                        programCounter += 4;
                    }
                    else
                    {
                        programCounter += 2;
                    }

                    return;

                case 0x6000:

                    // 6XNN set vRegsX is equal to kk (so 0x6kk in this case).
                    vRegs[(opcode & 0x0F00) >>> 8] = (ushort)(opcode & 0x00FF);

                    programCounter += 2;

                    return;

                case 0x7000:

                    // 7XNN adds NN to VX
                    x = (ushort)((opcode & 0x0F00) >>> 8);

                    ushort num = (ushort)(opcode & 0x00FF);

                    ushort sum = (ushort)(vRegs[x] + num);

                    // we have issues if we go above 256 (mem stuff) subtract 256 since we will go over the stack
                    if (sum >= 256)
                    {
                        vRegs[x] = (ushort)(sum - 256);
                    }
                    else
                    {
                        vRegs[x] = sum;
                    }

                    programCounter += 2;

                    return;
            }

            // More functions (we have to run them in a sepretate case, since they run in congjunction with the basic functions)
            switch (opcode & 0xF00F)
            {

                case 0x8000:

                    // 8XY0 sets vRegX to vRegY.
                    vRegs[(opcode & 0x0F00) >>> 8] = vRegs[(opcode & 0x00F0) >>> 4];

                    programCounter += 2;

                    return;

                case 0x8001:

                    // 8XY1 sets vRegX to vregY.
                    x = (ushort)((opcode & 0x0F00) >>> 8);
                    vRegs[x] = (ushort)(vRegs[x] | vRegs[(opcode & 0x00F0) >>> 4]);

                    programCounter += 2;

                    return;

                case 0x8002:

                    // 8XY2 sets vRegX to vRegY and vRegX.
                    x = (ushort)((opcode & 0x0F00) >>> 8);
                    vRegs[x] = (ushort)(vRegs[x] & vRegs[(opcode & 0x00F0) >>> 4]);

                    programCounter += 2;

                    return;

                case 0x8003:

                    // 8XY3 sets vRegX to vRegY XOR (XOR gate) vRegX. (^ is xor)
                    x = (ushort)((opcode & 0x0F00) >>> 8);
                    vRegs[x] = (ushort)(vRegs[x] ^ vRegs[(opcode & 0x00F0) >>> 4]);

                    programCounter += 2;

                    return;

                case 0x8004:

                    // 8XY4 sets vRegX to vRegX + vRegY, and sets vF to carry.
                    // according to gulrak, it is used for when overflow happenen, to 0 if not, even if X=F!
                    x = (ushort)((opcode & 0x0F00) >>> 8);

                    ushort sum = (ushort)(vRegs[x] + vRegs[(opcode & 0x00F0) >>> 4]);

                    // 0xFF is 255 
                    vRegs[x] = (ushort)(sum & 0xFF);
                    vRegs[0xF] = (ushort)(sum > 0xFF ? 1 : 0);


                    programCounter += 2;

                    return;

                case 0x8005:

                    // 8XX5 sets vRegY to vXReg - vYReg; then sets vF to not borrow if needed
                    // sets to 1 if if X is equal to F.

                    x = (ushort)((opcode & 0x0F00) >>> 8);
                    y = (ushort)((opcode & 0x00F0) >>> 4);

                    if (vRegs[y] > vRegs[x])
                    {
                        carry = 0;
                    }
                    else
                    {
                        carry = 1;
                    }

                    vRegs[x] = (ushort)((vRegs[x] - vRegs[y]) & 0xFF);

                    vRegs[0xF] = carry;

                    programCounter += 2;

                    return;

                case 0x8006:

                    // 8XX6 sets vRegX to vRegY and shifts vRegX a bit to the right (>>> yeah weird inverse), a
                    // and sets vF to 0 if and underflow happens like some other ops.
                    x = (ushort)((opcode & 0x0F00) >>> 8);

                    if ((vRegs[x] & 0x1) == 1)
                    {
                        carry = 1;
                    }
                    else
                    {
                        carry = 0;
                    }

                    vRegs[x] = (ushort)((vRegs[x]) >>> 1);

                    vRegs[0xF] = carry;

                    programCounter += 2;

                    return;

                case 0x8007:

                    // 0XX7 sets vRegX to the result of subtracting vRegX from vRegY, and yeah underflow stuff.
                    x = (ushort)((opcode & 0x0F00) >>> 8);
                    y = (ushort)((opcode & 0x00F0) >>> 4);

                    if (vRegs[y] > (ushort)((vRegs[y] - vRegs[x]) & 0xFF))
                    {
                        carry = 1;
                    }
                    else
                    {
                        carry = 0;
                    }

                    vRegs[x] = (ushort)((vRegs[y] - vRegs[x]) & 0xFF);

                    vRegs[0xF] = carry;

                    programCounter += 2;

                    return;

                case 0x800E:

                    // 8XYE shifts vRegX a bit to the left, even if X==F.
                    x = (ushort)((opcode & 0x0F00) >>> 8);

                    if ((vRegs[x] >>> 7) == 0x1)
                    {
                        carry = 1;
                    }
                    else
                    {
                        carry = 0;
                    }

                    vRegs[x] = (ushort)((vRegs[x] << 1) & 0xFF);

                    vRegs[0xF] = carry;

                    programCounter += 2;

                    return;

                case 0x9000:

                    // 9XY0 skips the next instruction if vRegX is not equal to vRegY
                    x = (ushort)((opcode & 0x0F00) >>> 8);

                    if (vRegs[x] != vRegs[(opcode & 0x00F0) >>> 4])
                    {
                        programCounter += 4;
                    }
                    else
                    {
                        programCounter += 2;
                    }

                    return;

            }

            // Graphics RNG & Some Regs
            switch (opcode & 0xF000)
            {

                case 0xA000:

                    // ANNN sets indexReg to nnn
                    indexReg = (ushort)(opcode & 0x0FFF);

                    programCounter += 2;

                    return;

                case 0xB000:

                    // BNNN jumps to nnnn + vReg at 0
                    programCounter = (ushort)((opcode & 0x0FFF) + vRegs[0]);

                    return;

                case 0xC000:

                    // CXNN sets vRegX to a random byte from NN
                    x = (ushort)((opcode & 0x0F00) >>> 8);

                    vRegs[x] = (ushort)((random.Next(256)) & (opcode & 0x00FF));

                    programCounter += 2;

                    return;

                case 0xD000:

                    // DXNN draws a 8xN pixel on screen.
                    // We map it to our SetPixelArea

                    y = vRegs[(opcode & 0x00F0) >> 4];
                    x = vRegs[(opcode & 0x0F00) >> 8];

                    int height = opcode & 0x000F;

                    vRegs[0xF] = 0;

                    for (int yLine = 0; yLine < height; yLine++)
                    {

                        int pixel = memory[indexReg + yLine];

                        for (int xLine = 0; xLine < 8; xLine++)
                        {

                            // we must check each pixel (bit) in a 8 bit row
                            if ((pixel & (0x80 >> xLine)) != 0)
                            {

                                int xCoord = x + xLine; // wrapping
                                int yCoord = y + yLine; // wraping

                                if (xCoord < 64 && yCoord < 32)
                                {

                                    // if we need to carry
                                    if (_screen.CheckPixelState(xCoord, yCoord) == true)
                                    {
                                        vRegs[0xF] = 1;
                                    }


                                    _screen.SetPixel(yCoord, xCoord);
                                }

                            }
                        }
                    }

                    drawFlag = true;
                    programCounter += 2;

                    return;
            }

            // Keyboard More Graphics & More 
            switch (opcode & 0xF0FF)
            {

                case 0xE09E:

                    // EX9E skips next opcode if key in the lower 4 bits of vX is pressed (gulrak says platforms that have 4 byte opcodes, like F000 on XO-CHIP, this needs to skip four bytes).
                    // The array itselfs contains key states, that is set in Keyboard8.cs thankgod, we dont't have to map xna input.
                    if (_keyboard.IsKeyPressed(vRegs[(opcode & 0x0F00) >>> 8]))
                    {
                        programCounter += 4;
                    }
                    else
                    {
                        programCounter += 2;
                    }

                    return;

                case 0xE0A1:

                    // EXA1 is just the oppsite of 0xE09E, it skips the next instruction if key with the value of vRegX is not pressed;
                    if (_keyboard.IsKeyPressed(vRegs[(opcode & 0x0F00) >>> 8]))
                    {
                        programCounter += 4;
                    }
                    else
                    {
                        programCounter += 2;
                    }

                    return;

                case 0xF007:

                    // FX07 sets vRegX to the delay timer value.
                    x = (ushort)((opcode & 0x0F00) >>> 8);

                    vRegs[x] = (ushort)(delayTimer & 0xFF);

                    programCounter += 2;

                    return;

                case 0xF00A:

                    // FX0A waits for a key press and stores the value of k in vRegX;
                    x = (ushort)((opcode & 0x0F00) >>> 8);

                    for (ushort s = 0; s <= 0xF; s++)
                    {
                        if (_keyboard.IsKeyPressed(s))
                        {

                            vRegs[x] = s;

                            programCounter += 2;


                            return;
                        }
                    }

                    return;

                case 0xF015:

                    // FX15 sets delay timer to vRegX
                    x = (ushort)((opcode & 0x0F00) >>> 8);

                    delayTimer = vRegs[x];

                    programCounter += 2;

                    return;

                case 0xF018:

                    // FX18 sets sound timer to vRegX
                    x = (ushort)((opcode & 0x0F00) >>> 8);

                    soundTimer = vRegs[x];

                    programCounter += 2;

                    return;

                case 0xF01E:

                    // FX1E set regIndex to 1 when there's a range overflow 
                    x = (ushort)((opcode & 0x0F00) >>> 8);

                    if (indexReg + vRegs[x] > 0xFFF)
                    {
                        carry = 1;
                    }
                    else
                    {
                        carry = 0;
                    }

                    indexReg = (ushort)((indexReg + vRegs[x]) & 0xFFF);

                    vRegs[0xF] = carry;

                    programCounter += 2;

                    return;

                case 0xF029:

                    // FX29 sets the indexReg to the loction of sprite for vRegX
                    x = (ushort)((opcode & 0x0F00) >>> 8);

                    indexReg = (ushort)(vRegs[x] * 5);
                    drawFlag = true;


                    programCounter += 2;

                    return;

                case 0xF033:

                    // FX33 stores binary coded deceimals in rep of vRegX in mem indexReg, indexReg + 1, and indexReg + 2
                    x = (ushort)((opcode & 0x0F00) >>> 8);

                    memory[indexReg] = (ushort)(vRegs[x] / 100);

                    memory[indexReg + 1] = (ushort)((vRegs[x] % 100) / 10);
                    memory[indexReg + 2] = (ushort)((vRegs[x] % 100) % 10);

                    programCounter += 2;

                    return;

                case 0xF055:

                    // FX55 store registers vReg0 through vRegX in memory starting at location regIndex.
                    x = (ushort)((opcode & 0x0F00) >>> 8);

                    for (int s = 0; s <= x; s++)
                    {
                        memory[indexReg + s] = vRegs[s];
                    }

                    programCounter += 2;

                    return;

                case 0xF065:

                    // FX65 reads registers vReg0 through vRegX from memory starting at a location.
                    x = (ushort)((opcode & 0x0F00) >>> 8);

                    for (int s = 0; s <= x; s++)
                    {
                        vRegs[s] = (ushort)(memory[indexReg + s] & 0xFF);
                    }

                    programCounter += 2;

                    return;

            }


        }

        /// <summary>
        /// Method <c>Update</c> Updates duh?
        /// </summary>
        public override void Update()
        {
            SetOpcode();
            CPUStep();

            // Update Timers
            if (delayTimer > 0)
            {
                delayTimer -= 1;
            }

            if (soundTimer > 0)
            {

                if (soundTimer == 1)
                {
                    _sound.PlaySoundWave(60_000, 1_000, SoundWaveType.NOISE, 1f);
                }

                soundTimer -= 1;
            }

        }

    }
}