using System;
using System.Collections.Generic;
using System.Text;

namespace Indicators
{
    public class IndicatorsEngine
    {
        public static void Process(IndicatorsControlsState controlState)
        {
            uint currentTimeStamp = controlState.counterMs;

            // resetting flashIndicatorTimeStamp to 0 will trigger isIndicatorActive on first run
            Action updateIndicator = () =>
            {
                controlState.lastIndicator = controlState.nextIndicator;
                controlState.lastIndicatorTimeStamp = controlState.nextIndicatorKeyEventTimeStamp;

                switch (controlState.lastIndicator)
                {
                    case eIndicatorType.Left:
                        controlState.slideValue = 0;
                        break;
                    case eIndicatorType.Right:
                        controlState.slideValue = 15;
                        break;
                    default:
                        controlState.slideValue = 0;
                        break;
                }
            };

            Action resetBlinker = () =>
            {
                controlState.flashIndicatorTimeStamp = 0;
                controlState.isIndicatorActive = false;
            };

            Action setBlinker = () =>
            {
                controlState.flashIndicatorTimeStamp = currentTimeStamp;
                controlState.isIndicatorActive = true;
            };

            Action toggleBlinker = () =>
            {
                controlState.flashIndicatorTimeStamp = currentTimeStamp;
                controlState.isIndicatorActive = !controlState.isIndicatorActive;
            };

            Action resetAutoBlinking = () =>
            {
                controlState.autoIndicatorTimeStamp = 0;
            };

            Action setAutoBlinking = () =>
            {
                controlState.autoIndicatorTimeStamp = controlState.lastIndicatorTimeStamp;
            };

            if (controlState.lastIndicator != controlState.nextIndicator)
            {
                // indicator was changed
                if (controlState.autoIndicatorTimeStamp != 0)
                {
                    // running on auto indicator
                    if (controlState.nextIndicator == eIndicatorType.None)
                    {
                        // no key pressed
                        if (
                            // was break signal
                            (controlState.lastIndicator == eIndicatorType.Break) ||
                            // timed out and wait for not active
                            (!controlState.isIndicatorActive &&
                            (controlState.slideValue == 0 || controlState.slideValue == 15) &&
                            currentTimeStamp - controlState.autoIndicatorTimeStamp >= 3000))
                        {
                            // turn off auto indicator after 3 seconds and when it is off to avoid quick blinks
                            resetAutoBlinking();
                            updateIndicator();
                        }

                        // do nothing if running on auto and nothing changed
                    }
                    else
                    {
                        // different indicator was pressed, reset auto state
                        resetAutoBlinking();
                        updateIndicator();
                        setBlinker();
                    }
                }
                else
                {
                    if (controlState.nextIndicator == eIndicatorType.None && (controlState.lastIndicatorTimeStamp == 0 || (currentTimeStamp - controlState.lastIndicatorTimeStamp) <= 500))
                    {
                        // turn on auto indicator if key was pressed less then 500 ms
                        setAutoBlinking();
                    }
                    else
                    {
                        updateIndicator();
                        setBlinker();
                    }
                }
            }

            // blinking logic
            switch (controlState.lastIndicator)
            {
                case eIndicatorType.None:
                    controlState.isIndicatorActive = false;
                    break;
                case eIndicatorType.Break:
                    controlState.isIndicatorActive = true;
                    break;
                default:
                    if (controlState.flashSpeedMs != 0 &&
                        (currentTimeStamp - controlState.flashIndicatorTimeStamp >= controlState.flashSpeedMs))
                    {
                        toggleBlinker();
                    }
                    break;
            }
        }
    }
}
