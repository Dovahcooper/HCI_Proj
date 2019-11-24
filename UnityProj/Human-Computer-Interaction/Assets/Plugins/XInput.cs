using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace XBOX
{
    public struct Stick
    {
        public float xAxis;
        public float yAxis;
    }

    public struct Triggers
    {
        public float lTrig;
        public float rTrig;
    }

    public enum Buttons
    {
        A = 0x5800,
        B = 0x5801,
        X = 0x5802,
        Y = 0x5803,
        RB = 0x5804,
        LB = 0x5805,
        LTrig = 0x5806,
        RTrig = 0x5807,

        DPadUp = 0x5810,
        DPadDown = 0x5811,
        DPadLeft = 0x5812,
        DPadRight = 0x5813,
        Start = 0x5814,
        Select = 0x5815,
        L3 = 0x5816,
        R3 = 0x5817,

        LS_Up = 0x5820,
        LS_Down = 0x5821,
        LS_Right = 0x5822,
        LS_Left = 0x5823,
        LS_Up_Left = 0x5824,
        LS_Up_Right = 0x5825,
        LS_Down_Right = 0x5826,
        LS_Down_Left = 0x5827,
    };


    public class XInput : MonoBehaviour
    {
        const string dllName = "XInput1_4 Wrapper";

        [DllImport(dllName)]
        private static extern void initDLL();

        [DllImport(dllName)]
        public static extern bool GetConnected(int _index = 0);

        [DllImport(dllName)]
        private static extern bool DownloadPackets(int num_controllers = 1);

        [DllImport(dllName)]
        private static extern void UpdateController(int _index = 0);

        [DllImport(dllName)]
        public static extern bool GetKeyPressed(int _index, int _button);

        [DllImport(dllName)]
        public static extern bool GetKeyReleased(int _index, int _button);

        [DllImport(dllName)]
        //public static extern Stick GetLeftStick(int _index = 0);
        public static extern float GetLeftX(int index = 0);

        [DllImport(dllName)]
        public static extern float GetLeftY(int index = 0);

        [DllImport(dllName)]
        public static extern float GetRightX(int index = 0);

        [DllImport(dllName)]
        public static extern float GetRightY(int index = 0);

        [DllImport(dllName)]
        public static extern Stick GetRightStick(int _index = 0);

        [DllImport(dllName)]
        //public static extern Triggers GetTriggers(int _index = 0);
        public static extern float GetLeftTrigger(int index = 0);

        [DllImport(dllName)]
        //public static extern Triggers GetTriggers(int _index = 0);
        public static extern float GetRightTrigger(int index = 0);

        [DllImport(dllName)]
        public static extern bool SetVibration(int _index = 0, float l_motor = 0.0f, float r_motor = 0.0f);

        [DllImport(dllName)]
        private static extern void cleanDLL();

        public static Buttons[] arr = new Buttons[]{
            Buttons.A,
            Buttons.B,
            Buttons.X,
            Buttons.Y,
            Buttons.RB,
            Buttons.LB,
            Buttons.RTrig,
            Buttons.LTrig,
            Buttons.DPadUp,
            Buttons.DPadDown,
            Buttons.DPadLeft,
            Buttons.DPadRight,
            Buttons.Start,
            Buttons.Select,
            Buttons.R3,
            Buttons.L3,
        };

        // Start is called before the first frame update
        void Start()
        {
            initDLL();
        }

        // Update is called once per frame
        void Update()
        {
            DownloadPackets();
            UpdateController();
        }

        private void OnDestroy()
        {
            cleanDLL();
        }

        public static bool AnyButton()
        {
            for(int i = 0; i < 16; i++)
            {
                if(GetKeyPressed(0, (int)arr[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public static void AnyButton(ref Buttons button)
        {
            for(int i = 0; i < 16; i++)
            {
                if(GetKeyPressed(0, (int)arr[i]))
                {
                    button = arr[i];
                }
            }
        }
    }
}