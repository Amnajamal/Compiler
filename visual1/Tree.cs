

/* <stmt>   :   =   vs  <ident> =   <expr>
                |   cs  <ident> =   <expr>
 *              |   register    <ident> =   <expr>
 *              |   int <ident> =   <expr>
 *              |   <ident> =   <expr>
 *              |   if  <cond>  do  <stmt>  end
 *              |   if  <cond>  do  <stmt>  else    do  <stmt>  end
 *              |   while   <cond>  do  <stmt>  end
 *              |   display    <expr>
 *              |   acquire    <ident>
 *              |   <stmt>  #   <stmt>
 *              */
public abstract class stmt
{
}

/* vs   <ident> =   <expr>
 * cs   <ident> =   <expr> 
 * register <ident> =   <expr>
 * int  <ident> =   <expr>
 *  */
public class DeclareS : stmt
{
    public string Ident;
    public expr Expr;
}

public class Just_Declare : stmt
{
    public string Ident;
}


//<ident>   =   <expr>
public class assign : stmt
{
    public string Ident;
    public expr Expr;
}


//display   <expr>
public class display : stmt
{
    public expr Expr;
}


//if    <cond>  do  <stmt>  end
public class ifS : stmt
{
    public expr Cond;
    public stmt Body;
}


//if    <cond>  do  <stmt>  else    do  <stmt>  end
public class ifElse : stmt
{
    public ifS ifEverything;
    //public stmt bodyIf;
    public stmt bodyelse;
}


//while <cond>  do  <stmt>  end
public class whileLoop : stmt
{
    public expr cond;
    public stmt body;
}


//acquire   <ident>
public class acquire : stmt
{
    public string Ident;
}


//<stmt> ; <stmt>
public class sequence : stmt
{
    public stmt first;
    public stmt second;
}


/*<expr>    :   =   <string>
 *              |   <int>
 *              |   <arith_expr>
 *              |   <ident>
 *              */
public abstract class expr
{
}


// <string> := " <string_elem>* "
public class StringLiteral : expr
{
    public string Value;
}

// <int> := <digit>+
public class IntLiteral : expr
{
    public int Value;
}

// <ident> := <char> <ident_rest>*
// <ident_rest> := <char> | <digit>
public class Variable : expr
{
    public string Ident;
}

public class Something : expr
{
    public BinExpr hello;

}

// <arith_expr> := <expr> <arith_op> <expr>
public class BinExpr : expr
{
    public expr Left;
    public expr Right;
    public BinOp Op;
}

// <arith_op> := + | - | * | /| ++ | || 
public enum BinOp
{
    add,
    minus,
    multiply,
    divide,
    series,
    parallel,
    equale,     //==
    greater,    //>
    note,        //~
    lesser,       //<
    greatere,   //>=
    lessere,       //<=
    empty

}


//<bool>    :   =   0   |   1
public class booleanL : expr
{
    public bool Value;
}

//<cond_expr>   :   =   <expr>  <cond_op>   <expr>
public class condExpr : expr
{
    public expr Left;
    public expr Right;
    public BinOp condop;
}


//<arith_expr> := <expr> <arith_op> <expr>
/*public class arithExpr : expr
{
    public expr Left;
    public expr Right;
    public BinOp Op;
}*/


//<cond_op> :   =   >   |   <   |   ==  |   >=  |   <=  |   ~
/*public enum condop
{
    equale,     //==
    greater,    //>
    note,        //~
    lesser,       //<
    greatere,   //>=
    lessere,       //<=
    empty
}
*/
