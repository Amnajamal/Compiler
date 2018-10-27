using Collections = System.Collections.Generic;
using Reflect = System.Reflection;
using Emit = System.Reflection.Emit;
using IO = System.IO;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

public sealed class CodeGen
{
    Emit.ILGenerator il = null;
    Collections.Dictionary<string, Emit.LocalBuilder> symbolTable;
    private int counter;
    private int counting;
    private stmt stmts;
    public IList<object> whole = new List<object>();
    public int counters;
    public CodeGen(stmt stmt, string moduleName, int count)
    {
        if (IO.Path.GetFileName(moduleName) != moduleName)
        {
            throw new System.Exception("can only output into current directory!");
        }
        stmts = stmt;

        Reflect.AssemblyName name = new Reflect.AssemblyName("FAJF"); //name of the assembly
        Emit.AssemblyBuilder asmb = System.AppDomain.CurrentDomain.DefineDynamicAssembly(name, Emit.AssemblyBuilderAccess.Save); 
        Emit.ModuleBuilder modb = asmb.DefineDynamicModule(moduleName);
        Emit.TypeBuilder typeBuilder = modb.DefineType("resister"); //name of the class

        Emit.MethodBuilder methb = typeBuilder.DefineMethod("Main", Reflect.MethodAttributes.Static, typeof(void), System.Type.EmptyTypes);

        // CodeGenerator
        this.il = methb.GetILGenerator();
        this.symbolTable = new Collections.Dictionary<string, Emit.LocalBuilder>();
        counting = 0;
        counter = count;
        counters = 0;
        
      

        // Go Compile!
        this.GenStmt(stmt);

        il.Emit(Emit.OpCodes.Ret);
        typeBuilder.CreateType();
        modb.CreateGlobalFunctions();
        asmb.SetEntryPoint(methb);
        asmb.Save(moduleName);
       // this.il.EmitWriteLine("press any key to continue");
        
        this.symbolTable = null;
        this.il = null;
        System.Diagnostics.Process.Start(moduleName);
    }


    private void GenStmt(stmt stmt)
    {
        if (stmt is sequence)
        {
            sequence seq = (sequence)stmt;
            this.GenStmt(seq.first);
            this.GenStmt(seq.second);
        }

        else if (stmt is DeclareS)
        {
            // declare a local
            counting++;
            DeclareS declare = (DeclareS)stmt;
            this.symbolTable[declare.Ident] = il.DeclareLocal(TypeOfExpr(declare.Expr));

            // set the initial value
            assign assign = new assign();
            assign.Ident = declare.Ident;
            assign.Expr = declare.Expr;
            this.GenStmt(assign);
            
            
        }

        else if (stmt is assign)
        {
            counting++;
            assign assign = (assign)stmt;
            counters = 0;
            this.GenExpr(assign.Expr, this.TypeOfExpr(assign.Expr));
            
            this.Store(assign.Ident, this.TypeOfExpr(assign.Expr));
           
        }
        else if (stmt is display)
        {
            // the "display" statement is an alias for System.Console.WriteLine. 
            // it uses the string case
            counting++;
            this.GenExpr(((display)stmt).Expr, typeof(string));
            
            this.il.Emit(Emit.OpCodes.Call, typeof(System.Console).GetMethod("WriteLine", new System.Type[] { typeof(string) }));

         
        }
        else if (stmt is Just_Declare)
        {
            counting++;
            Just_Declare declare = (Just_Declare)stmt;
            this.symbolTable[declare.Ident] = this.il.DeclareLocal(typeof(int));

           
            //this.Store(declare.Ident, typeof(int));
        }

        else if (stmt is acquire)
        {
            counting++;
            this.il.Emit(Emit.OpCodes.Call, typeof(System.Console).GetMethod("ReadLine", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, null, new System.Type[] { }, null));
            this.il.Emit(Emit.OpCodes.Call, typeof(int).GetMethod("Parse", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, null, new System.Type[] { typeof(string) }, null));
            this.Store(((acquire)stmt).Ident, typeof(int)); // stores the identifier in stack

        
            
        }
        else
        {
            throw new System.Exception("don't know how to gen a " + stmt.GetType().Name);
           
        }




    }

    private void Store(string name, System.Type type)
    {
        if (this.symbolTable.ContainsKey(name))
        {
            Emit.LocalBuilder locb = this.symbolTable[name];

            if (locb.LocalType == type)
            {
                this.il.Emit(Emit.OpCodes.Stloc, this.symbolTable[name]);
               
            }
            else
            {
                throw new System.Exception("'" + name + "' is of type " + locb.LocalType.Name + " but attempted to store value of type " + type.Name);
            }
        }
        else
        {
            throw new System.Exception("undeclared variable '" + name + "'");
        }
    }

    private void ADD_EMIT()
    {
        
        //this.il.Emit(Emit.OpCodes.Ldc_I4, ((IntLiteral)whole[0]).Value);
        //this.il.Emit(Emit.OpCodes.Ldc_I4, ((IntLiteral)whole[2]).Value);
        //this.il.Emit(Emit.OpCodes.Add);

        if (whole[0] is IntLiteral)

            this.il.Emit(Emit.OpCodes.Ldc_I4, ((IntLiteral)whole[0]).Value);//pushes the value of passed expression on integer type stack
        else
        {
            string ident = ((Variable)whole[0]).Ident;
            Emit.LocalBuilder locb = this.symbolTable[ident];
            this.il.Emit(Emit.OpCodes.Ldloc, locb);
        }

        for (int len = 2; len < whole.Count ; len=len+2)
        {

                if (whole[len] is IntLiteral)

                    this.il.Emit(Emit.OpCodes.Ldc_I4, ((IntLiteral)whole[len]).Value);//pushes the value of passed expression on integer type stack
                else
                {
                    string ident = ((Variable)whole[len]).Ident;
                    Emit.LocalBuilder locb = this.symbolTable[ident];
                    this.il.Emit(Emit.OpCodes.Ldloc, locb);
                }
                
                

                if ((whole[len-1]).Equals(BinOp.add))
                {
                    this.il.Emit(Emit.OpCodes.Add);
                }
                else if ((whole[len-1]).Equals(BinOp.minus))
                {
                    this.il.Emit(Emit.OpCodes.Sub);
                }
                else if ((whole[len-1]).Equals(BinOp.multiply))
                {
                    this.il.Emit(Emit.OpCodes.Mul);
                }
                else if ((whole[len-1]).Equals(BinOp.divide))
                {
                    this.il.Emit(Emit.OpCodes.Div);
                }


            
            
        }
    }

    private void GenExpr(expr expr, System.Type expectedType)
    {
        System.Type deliveredType;

        if (expr is StringLiteral)
        {
            deliveredType = typeof(string);
            this.il.Emit(Emit.OpCodes.Ldstr, ((StringLiteral)expr).Value);  //pushes the value of passed expression on string type stack
           // il.EmitWriteLine("hello");
        }
        else if (expr is IntLiteral)
        {
            deliveredType = typeof(int);
            this.il.Emit(Emit.OpCodes.Ldc_I4, ((IntLiteral)expr).Value); //pushes the value of passed expression on integer type stack
        }
        else if (expr is Variable)
        {
            string ident = ((Variable)expr).Ident;
            deliveredType = this.TypeOfExpr(expr);

            if (!this.symbolTable.ContainsKey(ident))
            {
                throw new System.Exception("undeclared variable '" + ident + "'");
            }

            this.il.Emit(Emit.OpCodes.Ldloc, this.symbolTable[ident]);
            
        }
        else if (expr is BinExpr)
        {
            BinExpr val = new BinExpr();
            val = (BinExpr)expr;
            deliveredType = typeof(int);


                whole.Add(val.Left);
                counter++;
                //MessageBox.Show(whole.Count().ToString());
                whole.Add(val.Op);
                //counters++;
                //MessageBox.Show(whole.Count().ToString());
                
           
            
            if ((TypeOfExpr((expr)val.Right) == typeof(int) && (!(val.Right is BinExpr))) || val.Right is Variable)
            {
                whole.Add(val.Right);
                //MessageBox.Show(whole.Count().ToString());
                ADD_EMIT();
            }
            else
            {
                
                this.GenExpr(val.Right, this.TypeOfExpr(val.Right));
            }
            
            
            
            /*if (val.Left is IntLiteral)

                this.il.Emit(Emit.OpCodes.Ldc_I4, ((IntLiteral)val.Left).Value);//pushes the value of passed expression on integer type stack
            else
            {
                string ident = ((Variable)val.Left).Ident;
                Emit.LocalBuilder locb = this.symbolTable[ident];
                this.il.Emit(Emit.OpCodes.Ldloc, locb);
            }

           
            if ( (TypeOfExpr((expr)val.Right)== typeof(int) && (! (val.Right is BinExpr))) || val.Right is Variable)
            {
            


                MessageBox.Show("blah blah");
                
                if (val.Right is IntLiteral)

                    this.il.Emit(Emit.OpCodes.Ldc_I4, ((IntLiteral)val.Right).Value);//pushes the value of passed expression on integer type stack
                else
                {
                    string ident = ((Variable)val.Right).Ident;
                    Emit.LocalBuilder locb = this.symbolTable[ident];
                    this.il.Emit(Emit.OpCodes.Ldloc, locb);
                }

                if ((val.Op).Equals(BinOp.add))
                {
                    this.il.Emit(Emit.OpCodes.Add);
                }
                else if ((val.Op).Equals(BinOp.minus))
                {
                    this.il.Emit(Emit.OpCodes.Sub);
                }
                else if ((val.Op).Equals(BinOp.multiply))
                {
                    this.il.Emit(Emit.OpCodes.Mul);
                }
                else if ((val.Op).Equals(BinOp.divide))
                {
                    this.il.Emit(Emit.OpCodes.Div);
                }

            }
            else 
            {
                this.GenExpr(val.Right, this.TypeOfExpr(val.Right));
            }*/

        }
        else
        {
            throw new System.Exception("don't know how to generate " + expr.GetType().Name);
        }

        if (deliveredType != expectedType) //type cheking 
        {
            if (deliveredType == typeof(int) &&
                expectedType == typeof(string))
            {
                this.il.Emit(Emit.OpCodes.Box, typeof(int));
                this.il.Emit(Emit.OpCodes.Callvirt, typeof(object).GetMethod("ToString")); //parsing integer to string
                //il.EmitCall(Emit.OpCodes.new
            }
            else
            {
                throw new System.Exception("can't coerce a " + deliveredType.Name + " to a " + expectedType.Name);
            }
        }

    }



    private System.Type TypeOfExpr(expr expr)
    {
        if (expr is StringLiteral)
        {
            return typeof(string); //setting the type of expr to type string
        }
        else if (expr is IntLiteral)
        {
            return typeof(int); //type to int
        }
        else if (expr is Variable)
        {
            Variable var = (Variable)expr;
            if (this.symbolTable.ContainsKey(var.Ident))
            {
                Emit.LocalBuilder locb = symbolTable[var.Ident]; //loacal variable assigned the value of symbol table at key var.Ident
                return locb.LocalType;
            }
            else
            {
                
                throw new System.Exception("undeclared variable '" + var.Ident + "'");
                
            }
        }
        else if (expr is BinExpr)
        {
            BinExpr var = (BinExpr)expr;
            return typeof(int);
        }
        
        else
        {
            
            throw new System.Exception("don't know how to calculate the type of " + expr.GetType().Name);
        }
    }
}