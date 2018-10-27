using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace visual1
{
    #region PARSER
    #region idnetifier_check class

    public class identifier_check
    {
        public IList<bool> assignment;
        public IList<string> identity;

        public identifier_check()
        {
            assignment = new List<bool>();
            identity = new List<string>();
        }

    }
    #endregion
    #region Errorlist class
    
    public class errorlist
    {
        public string error;
        public string ln;
        public errorlist(string err, int l)
        {
            error = err;
            ln = (l).ToString();
        }

    }
   
   
    #endregion

    public partial class parser
    {

        public int index;
        public IList<object> tokens;
        private readonly stmt result;
        public identifier_check identifier;
        public int lc = 0;
        public int counts;
        // public IList<scanner.estore> elist = new List<scanner.estore>();
         public IList <errorlist> errors = new List<errorlist>();

        #region add errors function
        public void adderror(string x)
        {
            errors.Add(new errorlist(x, lc));
        }
        #endregion

        public stmt Result
        {
            get
            {
                return result;
            }


        }
        public parser(IList<object> amna)
        {
            tokens = amna;
            index = 0;
            //elist = list;
            counts = 0;

            identifier = new identifier_check();


            #region Checking for Start
            if (tokens.Count != 0)
            {
                if (tokens[0].ToString().Equals("start"))
                {
                    index++;
                    linecount();
                }
                else
                {
                    // lc++;
                    adderror("expected start");
                }
            #endregion

                result = ParseStmt();
               // MessageBox.Show(counts.ToString());

                #region checking for proper end
                if (tokens[tokens.Count - 1].ToString().Equals("end"))
                {
                    index++;
                    linecount();
                   // MessageBox.Show("The first one");
                }
                else if (tokens[tokens.Count - 1].Equals((object)scanner.opend.newline) && tokens[tokens.Count - 2].ToString().Equals("end"))
                {
                    linecount();
                   // MessageBox.Show("The second one");
                }
                else
                {
                    adderror("EOF expected");
                  //  MessageBox.Show("The only error one");
                }

                /*   if (index != tokens.Count)
                    {
                        linecount();
                        MessageBox.Show("the last one" + tokens.Count.ToString() + index.ToString());

                        ////MessageBox.Show(tokens[index].ToString());
                        adderror("EOF expected");
                    }*/
                #endregion
            }
            ////MessageBox.Show(lc.ToString());
        }
        private void linecount()
        {
            if (tokens.Count != 0 && !(index >= tokens.Count))
            {
                if (tokens[index].Equals(scanner.opend.newline))
                {

                    lc++;
                    ////MessageBox.Show("new line here at " + (tokens[index - 1]).ToString() + lc.ToString());
                    if (index != tokens.Count)
                        index++;

                }
            }
        }


        private stmt ParseStmt()
        {
            stmt result;
            #region newline
            linecount();
            #endregion

            if (index < tokens.Count - 1)
            {
                #region DISPLAY
                if (index != tokens.Count - 1 && tokens[index].ToString().Equals("display"))
                {
                    //   linecount();
                    // ////MessageBox.Show("i am in display");
                    display Display = new display();
                    index++;
                    linecount();
                    Display.Expr = ParsExpr("blah");
                    if (Display.Expr is StringLiteral || Display.Expr is Variable)
                    {
                        if (Display.Expr is Variable)
                        {
                            //MessageBox.Show(tokens[index - 1].ToString());
                            if (!identifier.identity.Contains(tokens[index - 1].ToString()))
                            {
                                
                                adderror("undeclared identifier");
                                linecount();
                                // index++;
                            }
                            else if (identifier.assignment[identifier.identity.IndexOf(tokens[index - 1].ToString())] == false)
                            {
                                adderror(tokens[index - 1].ToString() + " : unassigned");
                                linecount();
                                // index++;
                            }
                        }
                        if (!tokens[index].Equals(scanner.opend.endline))
                        {
                            adderror("Expected Endline Character");
                            index--;
                        }
                        //else
                        //{
                        index++;
                        linecount();
                        //}
                        result = Display;
                        //  ////MessageBox.Show(tokens[index].ToString());

                    }
                    else
                    {
                        result = null;
                        adderror("expected Variable or Display Statement");
                        index++;
                        linecount();

                        if (!tokens[index].Equals(scanner.opend.endline))
                        {
                            adderror("Expected Endline Character");
                            index--;
                            //index++;
                        }
                    }
                    counts++;
                }
                #endregion

                #region For vs and cs and register and int
                else if (tokens[index].ToString().Equals("vs") || tokens[index].ToString().Equals("cs") || tokens[index].ToString().Equals("register") || tokens[index].ToString().Equals("int") || tokens[index].ToString().Equals("string"))
                {
                    string dose = tokens[index].ToString();
                    index++;
                    linecount();

                    DeclareS sources = new DeclareS();
                    if (!(index < tokens.Count && tokens[index] is string))
                    {
                        adderror("expected variable name");
                        index++;
                        linecount();

                    }
                    else if (identifier.identity.Contains(tokens[index].ToString()))
                    {
                        adderror(tokens[index].ToString() + " : redefinition");
                        index++;
                        linecount();
                    }
                    else
                        sources.Ident = tokens[index].ToString();
                    index++;
                    linecount();

                    if ((index == tokens.Count) || (!(tokens[index].Equals((object)scanner.opend.equal)) && !(tokens[index].Equals(scanner.opend.endline))))
                    {
                        //////MessageBox.Show(tokens[index].ToString());
                        adderror("Expected #/=");
                        // index++;
                    }
                    #region var identifier
                    if (tokens[index].Equals((object)scanner.opend.endline))
                    {
                        Just_Declare classy = new Just_Declare();
                        classy.Ident = tokens[index - 1].ToString();
                        if (identifier.identity.Contains(tokens[index - 1].ToString()))
                        {
                            adderror(tokens[index - 1].ToString() + " : redefinition");
                            index++;
                            linecount();
                        }
                        identifier.identity.Add(classy.Ident.ToString());

                        identifier.assignment.Add(false);

                        result = classy;
                        index++;
                        linecount();

                    }
                    #endregion

                    else
                    {

                        index++;
                        linecount();

                        sources.Expr = ParsExpr("blah");
                        if (dose == "string")
                        {
                            //MessageBox.Show("hello world");
                            if (!(sources.Expr is StringLiteral))
                            {
                                adderror("expected string type");
                                index++;
                                sources = null;

                            }
                            else
                            {
                                //MessageBox.Show("lala");
                                identifier.identity.Add(sources.Ident.ToString());
                                identifier.assignment.Add(true);
                                MessageBox.Show("haha");
                                //index++;
                                MessageBox.Show(tokens[index].ToString());
                                
                            }
                            if (!tokens[index].Equals(scanner.opend.endline))
                            {
                                //////MessageBox.Show(tokens[index].ToString());
                                adderror("Exepected Endline Character");
                                index--;
                            }
                            index++;
                            linecount();
                            result = sources;
                        }
                        else
                        {
                            if (sources.Expr is StringLiteral)
                            {
                                adderror("expected variable or integer after '=' ");
                                //index++;
                            }
                            else if (sources.Expr is Variable)
                            {
                                if (!identifier.identity.Contains(tokens[index - 1].ToString()))
                                {
                                    //////MessageBox.Show();
                                    adderror(tokens[index - 1].ToString() + " undeclared identifier");
                                    //  index++;
                                }

                                else if (identifier.assignment[identifier.identity.IndexOf(tokens[index - 1].ToString())] == false)
                                {
                                    adderror(tokens[index - 1].ToString() + " : unassigned");
                                    // index++;
                                }
                            }

                            identifier.identity.Add(sources.Ident.ToString());
                            identifier.assignment.Add(true);
                            if (!tokens[index].Equals(scanner.opend.endline))
                            {
                                //////MessageBox.Show(tokens[index].ToString());
                                adderror("Exepected Endline Character");
                                index--;
                            }
                            index++;
                            linecount();

                            result = sources;
                        }
                    }
                    counts++;
                }
                #endregion

                #region for Acquire
                else if (tokens[index].ToString().Equals("acquire"))
                {
                    index++;
                    linecount();
                    acquire acquire = new acquire();
                    if (index < tokens.Count && tokens[index] is string)
                    {
                        acquire.Ident = tokens[index].ToString();
                        ////MessageBox.Show(tokens[index].ToString());
                        if (!identifier.identity.Contains(acquire.Ident.ToString()))
                        {
                            adderror("Expected declared Identifier");
                            //index++;
                        }
                        
                        result = acquire;
                        identifier.assignment[identifier.identity.IndexOf(tokens[index].ToString())] = true;
                        index++;
                        if (!tokens[index].Equals(scanner.opend.endline))
                        {
                            adderror("Expected Endline Character");
                            index--;
                            //index++;
                        }

                        linecount();
                    }
                    else
                    {
                        result = null;
                        adderror("Expected Identifier");
                    }
                    index++;
                    linecount();
                    counts++;
                }
                #endregion

                #region If and If Else
                else if (tokens[index].ToString().Equals("if"))
                {
                    bool ife = false;
                    index++;
                    linecount();
                    ifS ifs = new ifS();
                    ifElse ifelse = new ifElse();
                    ifs.Cond = ParseCond();
                    if (ifs.Cond is IntLiteral || ifs.Cond is Something || ifs.Cond is Variable)
                    {
                        adderror("Expected Conditional Statement");
                        //index++;
                    }
                    //index++;
                    if (index == tokens.Count || !(tokens[index].ToString().Equals("do")))
                    {
                        adderror("Expected do");
                        index--;
                    }
                    index++;
                    linecount();
                    ifs.Body = ParseStmt();
                    // ////MessageBox.Show(tokens[index].ToString() + " b4 else");
                    //index++;

                    #region ifelse
                    if (tokens[index].ToString().Equals("else"))
                    {
                        index++;
                        linecount();

                        ifelse.ifEverything = ifs;
                        if (index == tokens.Count || !(tokens[index].ToString().Equals("do")))
                        {
                            adderror("Expected do");
                            index--;
                        }
                        index++;
                        linecount();
                        ifelse.bodyelse = ParseStmt();
                        //index++;
                        if (index == tokens.Count || !(tokens[index].ToString().Equals("endelse")))
                        {
                            adderror("unterminated 'if else' body");
                            //index++;
                        }

                        ife = true;



                    }
                    #endregion
                    else if (index == tokens.Count || !(tokens[index].ToString().Equals("endif")))
                    {
                        result = null;
                        adderror("unterminated 'if' body");
                        index++;
                    }
                    if (!ife)
                    {
                        index++;
                        linecount();
                        if (index == tokens.Count || !(tokens[index].Equals((object)scanner.opend.endline)))
                        {
                            result = null;
                            adderror("Expected endline character #");
                            index--;
                        }
                        else
                            result = ifs;
                    }
                    else
                    {
                        index++;
                        linecount();
                        if (index == tokens.Count || !(tokens[index].Equals((object)scanner.opend.endline)))
                        {
                            result = null;
                            adderror("Expected endline character #");
                            index--;
                        }
                        else
                            result = ifelse;
                    }
                    index++;
                    linecount();
                    counts++;
                }
                #endregion

                #region While loop
                else if (tokens[index].ToString().Equals("while"))
                {
                    index++;
                    linecount();
                    whileLoop whilel = new whileLoop();
                    whilel.cond = ParseCond();
                    ////MessageBox.Show(tokens[index].ToString());
                    //index++;

                    if (index == tokens.Count || !(tokens[index].ToString().Equals("do")))
                    {
                        ////MessageBox.Show(tokens[index].ToString());
                        adderror("Expected do");
                        index--;
                    }
                    index++;
                    linecount();
                    //////MessageBox.Show(tokens[index].ToString());
                    whilel.body = ParseStmt();
                    ////MessageBox.Show(tokens[index].ToString());
                    //index++;
                    if (index == tokens.Count || !(tokens[index].ToString().Equals("endwhile")))
                    {
                        adderror("unterminated 'while' loop body");
                    }
                    index++;
                    linecount();
                    if (index == tokens.Count || !(tokens[index].Equals((object)scanner.opend.endline)))
                    {
                        adderror("Expected endline #");
                        index--;
                    }
                    result = whilel;
                    index++;
                    linecount();
                    counts++;
                    ////MessageBox.Show(tokens[index].ToString()+" in while");
                }
                #endregion

                #region Assignment
                else if (tokens[index] is string)
                {
                    assign assign = new assign();
                    if (!identifier.identity.Contains(tokens[index].ToString()))
                        adderror(tokens[index] + " undeclared identifier");

                    assign.Ident = tokens[index].ToString();
                    index++;
                    linecount();



                    if (tokens.Count == index || !(tokens[index].Equals((object)scanner.opend.equal)))
                    {
                        adderror("expected '='");
                    }
                    index++;
                    linecount();
                    assign.Expr = ParsExpr("blah");
                    if (assign.Expr is StringLiteral)
                        adderror("Expected integer/variable");
                    else if (assign.Expr is Variable)
                    {
                        if (!identifier.identity.Contains(tokens[index - 1].ToString()))
                        {

                            adderror(tokens[index - 1] + " : undeclared identifier");
                        }

                        else if (identifier.assignment[identifier.identity.IndexOf(tokens[index - 1].ToString())] == false)
                        {
                            adderror(tokens[index - 1].ToString() + " : unassigned");
                        }
                    }

                    result = assign;
                    if (index < tokens.Count)
                    {
                        if (tokens[index].ToString().Equals("#"))
                        {
                            adderror("Expected EndLine Character");
                            index--;

                        }
                    }
                    //index++;
                    counts++;
                }
                #endregion

                #region Invalid Statement

                else
                {
                    linecount();
                    adderror("parse error at token " + index + ": " + tokens[index]);
                    result = ParseStmt();

                }
                #endregion

                #region statement list

                //MessageBox.Show("b4 stmt_list");
                if ((index < tokens.Count) && (!(tokens[index].ToString().Equals("end"))))
                {
                    // index++;
                    if (((!tokens[index].ToString().Equals("endwhile") && !tokens[index].ToString().Equals("endif"))) && (!tokens[index].ToString().Equals("endelse") && !tokens[index].ToString().Equals("else")))
                    {
                        //MessageBox.Show("i m in stmt_list");
                        linecount();
                        //MessageBox.Show(tokens[index ].ToString());


                        sequence seq = new sequence();
                        
                        seq.first = result;
                        seq.second = ParseStmt();
                        result = seq;
                    }
                }


                //}

                #endregion


                return result;
            }
            else return null;
        }
        private expr ParsExpr(string blah)
        {
            if (index >= tokens.Count)
            {
                adderror("Got EOF, and i expected an expression idiot");
                return null;

            }
            else if (tokens[index] is StringBuilder)
            {
                string value = ((StringBuilder)tokens[index]).ToString();
                index++;
                linecount();
                StringLiteral stringLiteral = new StringLiteral();
                stringLiteral.Value = value;
                return stringLiteral;

            }
            else if (tokens[index] is int)
            {
                int intValue = (int)tokens[index];
                index++;
                linecount();
                IntLiteral intLiteral = new IntLiteral();
                intLiteral.Value = intValue;
                BinOp x = ParseA_op();
                //MessageBox.Show(x.ToString());

                if (x.Equals(BinOp.empty))
                {
                    //MessageBox.Show("ninop");
                    return intLiteral;
                }
                else
                {
                    // if (x.Equals(BinOp.add) || x.Equals(BinOp.divide) || x.Equals(BinOp.minus) || x.Equals(BinOp.multiply) || x.Equals(BinOp.parallel) || x.Equals(BinOp.series))
                    //{
                    BinExpr thing = new BinExpr();
                    if (!(x.Equals(BinOp.add) || x.Equals(BinOp.divide) || x.Equals(BinOp.minus) || x.Equals(BinOp.multiply) || x.Equals(BinOp.parallel) || x.Equals(BinOp.series)))
                    {
                        if (blah.Equals("con"))
                            return thing;
                        else
                            adderror("Expected Operator");


                    }
                    thing.Left = (expr)intLiteral;
                    thing.Op = x;
                    index++;
                    linecount();
                    thing.Right = ParsExpr(blah);
                    //MessageBox.Show(thing.Right.ToString());
                    if (thing.Right is Variable)
                    {
                        if (!identifier.identity.Contains(tokens[index - 1].ToString()))
                        {
                            adderror(tokens[index - 1].ToString() + ": undeclared identifier");
                            // index++;
                        }
                        else if (identifier.assignment[identifier.identity.IndexOf(tokens[index - 1].ToString())] == false)
                        {
                            adderror(tokens[index - 1].ToString() + " : unassigned");
                            //index++;
                        }

                    }
                    // return thing;
                    // }
                    /*  if (!(x.Equals(BinOp.add) || x.Equals(BinOp.divide) || x.Equals(BinOp.minus) || x.Equals(BinOp.multiply) || x.Equals(BinOp.parallel) || x.Equals(BinOp.series)))
                  {
                      adderror("Expected Operator");
                     // //MessageBox.Show("hello ");

                      ////MessageBox.Show(thing.Left.ToString() + " " + thing.Op.ToString() + " " + thing.Right.ToString());
                      return thing;
                        
                  }*/
                    return thing;



                }



            }
            else if (tokens[index] is string)
            {
                string ident = (string)tokens[index];
                index++;
                linecount();
                Variable var = new Variable();
                var.Ident = ident;
                BinOp x = ParseA_op();
                if (x.Equals(BinOp.empty))
                {
                    return var;
                }
                else
                {
                    BinExpr thing = new BinExpr();



                    thing.Left = (expr)var;
                    if (!identifier.identity.Contains(var.Ident))
                    {
                        adderror(tokens[index - 1].ToString() + ": undeclared identifier");
                        //index++;
                    }


                    thing.Op = x;
                    index++;
                    linecount();
                    thing.Right = ParsExpr("blah");
                    if (thing.Right is Variable)
                    {
                        if (!identifier.identity.Contains(tokens[index - 1].ToString()))
                        {
                            adderror(tokens[index - 1].ToString() + ": undeclared identifier");

                        }

                        else if (identifier.assignment[identifier.identity.IndexOf(tokens[index - 1].ToString())] == false)
                        {
                            adderror(tokens[index - 1].ToString() + " : unassigned");
                        }

                    }
                    //return thing;

                    if (!(x.Equals(BinOp.add) || x.Equals(BinOp.divide) || x.Equals(BinOp.minus) || x.Equals(BinOp.multiply) || x.Equals(BinOp.parallel) || x.Equals(BinOp.series)))
                    {
                        //MessageBox.Show("binop add");
                        adderror("Expected Operator");
                        return thing;
                        // return null;
                    }
                    return thing;
                    //index--;

                }

            }
            else
            {
                //MessageBox.Show("i am in string literal");
                //MessageBox.Show(tokens[index].ToString());

                adderror("expected string literal, int literal or variable");
                return null;
            }
        }
        private BinOp ParseA_op()
        {
            if (index == tokens.Count)
            {
                return BinOp.empty;
            }
            if (tokens[index].Equals((object)scanner.opend.add))
            {
                //MessageBox.Show("hey dudes");
                return (BinOp.add);
            }
            else if (tokens[index].Equals((object)scanner.opend.minus))
            {
                return (BinOp.minus);
            }
            else if (tokens[index].Equals((object)scanner.opend.multiply))
            {
                return (BinOp.multiply);
            }
            else if (tokens[index].Equals((object)scanner.opend.divide))
            {
                return (BinOp.divide);
            }

            else if (tokens[index].Equals((object)scanner.opend.series))
            {
                return (BinOp.series);
            }
            else if (tokens[index].Equals((object)scanner.opend.equale))
            {
                return (BinOp.equale);
            }
            else if (tokens[index].Equals((object)scanner.opend.greater))
            {
                return (BinOp.greater);
            }
            else if (tokens[index].Equals((object)scanner.opend.greatere))
            {
                return (BinOp.greatere);
            }
            else if (tokens[index].Equals((object)scanner.opend.lesser))
            {
                return (BinOp.lesser);
            }
            else if (tokens[index].Equals((object)scanner.opend.lessere))
            {
                return (BinOp.lessere);
            }
            else if (tokens[index].Equals((object)scanner.opend.note))
            {
                return (BinOp.note);
            }
            else if (tokens[index].Equals((object)scanner.opend.parallel))
            {
                return (BinOp.parallel);
            }
            else if (tokens[index].ToString().Equals("="))
            {
                adderror("maybe you should put a == instead of the =");
                return (BinOp.equale);
            }



            else
            {
                return BinOp.empty;
            }
        }
        private expr ParseCond()
        {
            if (index == tokens.Count)
            {
                adderror("Got EOF, and i expected an expression idiot");
            }
            if (tokens[index] is StringBuilder)
            {
                adderror("expected intger/variable/condition");
                return null;

            }
            else if (tokens[index] is int)
            {
                int intValue = (int)tokens[index];
                index++;
                linecount();
                IntLiteral intLiteral = new IntLiteral();
                intLiteral.Value = intValue;
                BinOp x = ParseA_op();

                if (x.Equals(BinOp.empty))
                {
                    return intLiteral;
                }
                else
                {
                    BinExpr thing = new BinExpr();
                    if (x.Equals(BinOp.add) || x.Equals(BinOp.divide) || x.Equals(BinOp.minus) || x.Equals(BinOp.multiply) || x.Equals(BinOp.parallel) || x.Equals(BinOp.series))
                    {
                        index--;
                        //MessageBox.Show(tokens[index].ToString()+" "+tokens[index+1].ToString()+" "+tokens[index+2].ToString());
                        thing.Left = ParsExpr("con");
                        //MessageBox.Show("back");
                        //adderror("Expected Conditional Operator");
                    }

                    x = ParseA_op();
                    if (x.Equals(BinOp.empty))
                    {
                        Something blahness = new Something();
                        blahness.hello = (BinExpr)thing.Left;
                        //MessageBox.Show("i m in there");
                        return blahness;
                    }

                    thing.Left = (expr)intLiteral;
                    thing.Op = x;
                    index++;
                    linecount();
                    thing.Right = ParsExpr("blah");
                    if (thing.Right is Variable)
                    {
                        if (!identifier.identity.Contains(tokens[index - 1].ToString()))
                            adderror(tokens[index - 1].ToString() + ": undeclared identifier");

                        else if (identifier.assignment[identifier.identity.IndexOf(tokens[index - 1].ToString())] == false)
                        {
                            adderror(tokens[index - 1].ToString() + " : unassigned");
                        }

                    }
                    return thing;




                }


                // return intLiteral;


            }
            else if (tokens[index] is string)
            {
                string ident = (string)tokens[index];
                if (!identifier.identity.Contains(tokens[index].ToString()))
                    adderror(tokens[index].ToString() + " : undeclared identifier");
                else if (identifier.assignment[identifier.identity.IndexOf(tokens[index].ToString())] == false)
                {
                    adderror(tokens[index].ToString() + " : unassigned");
                }

                index++;
                linecount();
                Variable var = new Variable();
                var.Ident = ident;
                BinOp x = ParseA_op();
                if (x.Equals(BinOp.empty))
                {
                    return var;
                }
                else
                {
                    BinExpr thing = new BinExpr();
                    thing.Left = (expr)var;

                    if (x.Equals(BinOp.add) || x.Equals(BinOp.divide) || x.Equals(BinOp.minus) || x.Equals(BinOp.multiply) || x.Equals(BinOp.parallel) || x.Equals(BinOp.series))
                    {
                        index--;
                        thing.Left = ParsExpr("con");
                        // if (errors[errors.Count - 1].error.ToString().Equals("Expected Operator"))
                        //  errors.Remove(errors.Count - 1);
                        //adderror("Expected Conditional Operator");
                    }


                    if (!identifier.identity.Contains(var.Ident))
                        adderror(var.Ident + ": undeclared identifier");
                    else if (identifier.assignment[identifier.identity.IndexOf(var.Ident)] == false)
                    {
                        adderror(tokens[index].ToString() + " : unassigned");
                    }
                    x = ParseA_op();
                    if (x.Equals(BinOp.empty))
                    {
                        Something blahness = new Something();
                        blahness.hello = (BinExpr)thing.Left;
                        return blahness;
                    }
                    thing.Op = x;
                    index++;
                    linecount();
                    thing.Right = ParsExpr("blah");
                    if (thing.Right is Variable)
                    {
                        if (!identifier.identity.Contains(tokens[index - 1].ToString()))
                            adderror(tokens[index - 1].ToString() + ": undeclared identifier");

                        else if (identifier.assignment[identifier.identity.IndexOf(tokens[index - 1].ToString())] == false)
                        {
                            adderror(tokens[index - 1].ToString() + " : unassigned");
                        }
                        //index
                    }
                    return thing;
                }

            }

            else if (tokens[index] is bool)
            {
                bool value = (bool)tokens[index++];
                linecount();
                booleanL boolean = new booleanL();
                boolean.Value = value;
                return boolean;
            }
            else
            {
                adderror("expected string literal, int literal or variable");
                return null;
            }
        }

    }
    #endregion
}
