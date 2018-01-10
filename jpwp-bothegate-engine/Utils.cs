using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace jpwp_bothegate_engine
{
    /// <summary>
    /// static Class Utils
    /// Provides utilities for CLI debugging with formatting.
    /// </summary>
    public static class Utils
    {
        static Dictionary<int,string> drcErrWrnList = new Dictionary<int, string> {
            { 0,"Multiple drivers. Review the connections." },
            { 1,"Gate with floating input. Please connect inputs to a viable" +
                "source or remove the gate." },
            { 2,"Source is not connected." },
            { 3,"Output drives an input." },
            //512 is the break between Warnings and Errors
            { 532,"NAND used with 1 input." },
            //1024 is the break between Warnings and Info
            { 1024,"System was built correctly." }
            };
        public static void debugPrint(String s)
        {
            #if DEBUG
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("DEBUG:  " + s);
            Console.ForegroundColor = ConsoleColor.White;
            #else

            #endif
        }
        public static void infoPrint(String s)
        {
            #if DEBUG
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("INFO:   " + s);
                Console.ForegroundColor = ConsoleColor.White;
            #else

            #endif
        }

        public static void fdebugPrint(String s)
        {
            #if DEBUG
                using (
                    System.IO.StreamWriter file =
                    new System.IO.StreamWriter("report.log",true)
                    )
                    file.WriteLine(s);
            #else
                
            #endif
            }
        public static void clearReport()
        {
            #if DEBUG
                using (
                    System.IO.StreamWriter file =
                    new System.IO.StreamWriter("report.log", false)
                    )
                    file.WriteLine("REPORT LOG - BOTHEGATE KERNEL " + DateTime.Now.ToString("h:mm:ss tt") );
               
            #else
                
            #endif
            }
       public static void drcErrWrnMsg(String errSource,int code)
        {
                #if DEBUG
                String callType = "";
                if (drcErrWrnList.TryGetValue(code, out string value))
                {
                if(code < 512)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    callType = "ERROR";
                } else if(code < 1024){
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    callType = "WARNING";
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    callType = "INFO";
                }
                
                Console.WriteLine("DRC"+callType+":" + code + " " + drcErrWrnList[code]);
                Console.WriteLine(callType +" called by:" + errSource);
                Console.ForegroundColor = ConsoleColor.White;
                }
            #else
                
            #endif
        }
    }
}