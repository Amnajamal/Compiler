using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;


namespace visual1
{
    public partial class scanner
    {
        #region Complete Scanner

        private readonly IList<object> whole = new List<object>();
        public IList<errorlist> er = new List<errorlist>();
        int line = 0;

        /*  public class estore
          {
             public string before;
             public string after;
              public estore(string b, string a)
              {
                  before = b;
                  after = a;
              }
          }
          public IList<estore> list = new List<estore>();*/
        public scanner(char[] readI)
        {
            Scaning(readI);
        }

        public IList<object> Tokens
        {
            get { return whole; }
        }
        public enum opend
        {
            series,//++
            parallel,//||
            add,//+
            minus,//-
            divide,///
            multiply,//*
            equal,//=
            endline,//#
            greater,//>
            lesser,//<
            greatere,//>=
            lessere,//=<
            note,//~
            equale,//==
            newline// new line

        };
        // int n = 0;
        //int prevc=1;
        public void adderror(string x)
        {
            er.Add(new errorlist(x,line));
        }
        void Scaning(char[] readI)
        {
           
            for (int i = 0; i <= readI.Length; i++)
            {
                if (i == readI.Length)
                    break;
                char y = readI[i];
                bool quote = false;

                #region Whitespace Check and new line check
                if (char.IsWhiteSpace(y))
                {
                    if (y == '\n')
                    {
                        //StringBuilder add = new StringBuilder();
                        //add.Append(scanner.opend.newline);
                        //MessageBox.Show("new line");
                        whole.Add(opend.newline);
                        line++;
                    }



                    ////MessageBox.Show("WhiteSpace");
                    continue;
                }
                #endregion
                #region checking for string literal
                else if (y == '"')
                {
                    StringBuilder add = new StringBuilder();
                    y = readI[++i];
                    //add.Append('"');
                    quote = true;
                    while (y != '"')
                    {
                        add.Append(y);
                        ////MessageBox.Show("String literal "+ y.ToString());
                        i++;
                        if (i == readI.Length)
                        {
                            // i--;
                            break;
                        }
                        y = readI[i];
                    }
                    ////MessageBox.Show("Full thingi " + add);
                    if (y == '"')
                    {
                        quote = false;
                        whole.Add(add);


                        ////MessageBox.Show(add.ToString());
                    }
                    if (quote)
                    {

                        adderror("Put a \" here");
                    }

                    // //MessageBox.Show(add.ToString());
                }
                #endregion
                #region checking for comments
                else if (y == '$')
                {
                    y = readI[++i];
                    while (y != '\n')
                    {
                        i++;
                        if (i == readI.Length)
                        {
                            break;
                        }
                        y = readI[i];

                    }

                }
                #endregion
                #region checking for characters and _
                else if (char.IsLetter(y) || y == '_')
                {
                    StringBuilder add = new StringBuilder();
                    y = readI[i];
                    while (char.IsLetter(y) || y == '_')
                    {
                        add.Append(y);
                        ////MessageBox.Show("Syntax" + y.ToString());
                        i++;
                        if (i == readI.Length)
                        {
                            //i--;
                            break;
                        }
                        y = readI[i];
                    }
                    whole.Add(add.ToString());
                    //if (whole[0] is string)
                    //  //MessageBox.Show(whole[0].ToString());

                    i--;

                }
                #endregion
                #region checking for int literal
                else if (char.IsDigit(y))
                {
                    StringBuilder add = new StringBuilder();
                    y = readI[i];
                    while (char.IsDigit(y))
                    {
                        add.Append(y);
                        ////MessageBox.Show("Is Digit " + y.ToString());
                        i++;
                        if (i == readI.Length)
                        {
                            // i--;
                            break;
                        }
                        y = readI[i];
                    }
                    i--;
                    whole.Add(Int32.Parse(add.ToString()));

                    // if (whole[0] is Int32)
                    //  //MessageBox.Show("u are in there");
                    ////MessageBox.Show("Full thingi " + add);
                }
                #endregion

                #region operators and endline
                else
                {
                    int n = i + 1;
                    char o;
                    if (n != readI.Length)
                    {
                        o = readI[n];
                    }
                    else
                    {
                        o = ' ';
                    }
                    switch (y)
                    {
                        case '+':
                            {
                                if (o == '+')
                                {
                                    whole.Add(opend.series);

                                    // //MessageBox.Show("Series");
                                    ////MessageBox.Show(y.ToString() + o.ToString());
                                    i++;
                                }
                                else
                                {
                                    whole.Add(opend.add);

                                    ////MessageBox.Show("Plus");
                                    ////MessageBox.Show(y.ToString() + o.ToString());
                                }
                            }
                            break;
                        case '-':
                            {
                                whole.Add(opend.minus);

                            }
                            break;
                        case '/':
                            {
                                whole.Add(opend.divide);

                            }
                            break;
                        case '*':
                            {
                                whole.Add(opend.multiply);

                            }
                            break;
                        case '#':
                            {

                                whole.Add(opend.endline);
                            }
                            break;
                        case '~':
                            {
                                whole.Add(opend.note);

                            }
                            break;
                        case '=':
                            if (o == '=')
                            {
                                whole.Add(opend.equale);
                                i++;

                            }
                            else
                            {
                                whole.Add(opend.equal);

                            }
                            break;
                        case '>':

                            if (o == '=')
                            {
                                whole.Add(opend.greatere);
                                i++;

                            }
                            else
                            {
                                whole.Add(opend.greater);

                            }

                            break;
                        case '<':

                            if (o == '=')
                            {
                                whole.Add(opend.lessere);

                                i++;
                            }
                            else
                            {
                                whole.Add(opend.lesser);

                            }

                            break;
                        case '|':
                            if (o == '|')
                            {
                                whole.Add(opend.parallel);

                                i++;
                            }
                            break;
                        default:
                           adderror("Scanner Unrecognized Char" + y.ToString());
                           break;
                    }


                }
                #endregion
                #region checking for last line
                if ((i == readI.Length - 1) && (readI[readI.Length - 1] != '\n'))
                {
                    //StringBuilder add = new StringBuilder();
                    // add.Append(scanner.opend.newline);
                    whole.Add(opend.newline);
                    //MessageBox.Show("new line out box");
                    continue;
                }
                #endregion

            }
            //  for(int i=0;i<whole.Count;i++)
            //MessageBox.Show(whole[i].ToString()+(whole[i] is StringBuilder));
        }
        #endregion


    }
}
