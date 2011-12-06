using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX.DirectInput;

namespace robot
{
    class Gamepad
    {
        private Device _gamepad;
        public void Pripoj_Joystick()
        {
            DeviceList devlist = Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly);
            foreach (DeviceInstance inst in devlist)
            {
                Guid g = inst.InstanceGuid;
                _gamepad = new Device(g);
            }
            if (_gamepad != null)
            {
                _gamepad.SetDataFormat(DeviceDataFormat.Joystick);
                _gamepad.Acquire();
            }

        }
        public byte[] Tlacitka()
        {
            return _gamepad.CurrentJoystickState.GetButtons();
        }
        public int[] AxisXY()
        {
            int[] pomocny = new int[2];
            pomocny[0] = _gamepad.CurrentJoystickState.X;
            pomocny[1] = _gamepad.CurrentJoystickState.Y;
            return pomocny;
        }
        public bool IsConnected()
        {
            try
            {
                _gamepad.ToString();
                return true;
            }
            catch {
                return false;
            }
        }
        bool radio_status(byte hodnota)
        {
            if (hodnota == 128) return true;
            else return false;
        }
        int prevod_osi(int os, int calibrate, int calibrate0)
        {
            if (calibrate0 != os)
            {
                if (os == calibrate) return 0;
                else if (os < calibrate) return -1;
                else return 1;
            }
            else
            {
                return 2;
            }
        }
        string prepocet_smer(int osx, int osy)
        {
            if (osx == 0 & osy == -1) return "1";
            else if (osx == 1 & osy == -1) return "2";
            else if (osx == 1 & osy == 0) return "3";
            else if (osx == 1 & osy == 1) return "4";
            else if (osx == 0 & osy == 1) return "5";
            else if (osx == -1 & osy == 1) return "6";
            else if (osx == -1 & osy == 0) return "7";
            else if (osx == -1 & osy == -1) return "8";
            else return "0";
        }
        public string calc_gamepad(int calibrate, int calibrate0, int num_button)
        {
            int[] pomocny = AxisXY();
            byte[] pomocny_tlacitka = Tlacitka();
            int osx = prevod_osi(pomocny[0], calibrate, calibrate0);
            int osy = prevod_osi(pomocny[1], calibrate, calibrate0);
            bool all_tx_status = false;
            string zatlacene_tlacitko = "";
            for (int x = 0; x != num_button; x++)
            {
                if (radio_status(pomocny_tlacitka[x]) == true)
                {
                    all_tx_status = true;
                    zatlacene_tlacitko = (x + 1).ToString();
                }
            }
            if (all_tx_status == false) return prepocet_smer(osx, osy);
            else return "z_t_" + zatlacene_tlacitko;
        }
    }
}
