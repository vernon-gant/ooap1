public abstract class BoundedStack<T>
{
    //---Command result codes---

    public const int PUSH_NIL = 0; // push() has not been called yet
    public const int PUSH_OK = 1; // last push() completed successfully
    public const int PUSH_ERR = 2; // stack is full

    public const int POP_NIL = 0; // push() has not been called yet
    public const int POP_OK = 1; // last pop() completed successfully
    public const int POP_ERR = 2; // stack is empty

    public const int PEEK_NIL = 0; // push() has not been called yet
    public const int PEEK_OK = 1; // last peek() returned a valid result
    public const int PEEK_ERR = 2; // stack is empty

    public const int DEFAULT_MAX_CAPACITY = 32;

    //---Constructors---
    // must be implemented by every sub class

    // post condition : an empty stack with maximum capacity of "DEFAULT_MAX_CAPACITY" is created
    protected BoundedStack()
    {
        Clear();
    }

    // precondition : maxSize argument is a positive number
    // post condition : an empty stack with maximum capacity of "maxSize" is created
    protected BoundedStack(int maxCapacity) : this()
    {
        if (maxCapacity <= 0) throw new ArgumentException("maxSize must be a positive integer");
    }

    //---Commands---
    
    // precondition : current stack size is less than maximum capacity of the stack
    // post condition : new element is pushed on top of the stack
    public abstract void Push(T value);

    // precondition : stack is not empty
    // post condition : top element is deleted from stack
    public abstract void Pop();

    // post condition : all elements from the stack are deleted
    public abstract void Clear();

    //---Queries---

    // precondition : stack is not empty
    // for now return T? so that we can return something in case of an erro.
    // In future version will probably return some null object according to null object pattern
    public abstract T? Peek { get; }

    public abstract int Size { get; }

    //---Helper queries---

    public abstract int PushStatus { get; }

    public abstract int PopStatus { get; }

    public abstract int PeekStatus { get; }
}

public class ListBoundStack<T> : BoundedStack<T>
{
    // Hidden fields
    private List<T> _stack;
    private int _pushStatus;
    private int _popStatus;
    private int _peekStatus;
    private readonly int _maxCapacity = DEFAULT_MAX_CAPACITY;

    public ListBoundStack()
    {
        _stack = new List<T>(DEFAULT_MAX_CAPACITY);
    }

    public ListBoundStack(int maxCapacity) : base(maxCapacity)
    {
        _stack = new List<T>(maxCapacity);
        _maxCapacity = maxCapacity;
    }

    public override void Push(T value)
    {
        if (Size == _maxCapacity)
        {
            _pushStatus = PUSH_ERR;
            return;
        }

        _stack.Add(value);
        _pushStatus = PUSH_OK;
    }

    public override void Pop()
    {
        if (Size == 0)
        {
            _popStatus = POP_ERR;
            return;
        }

        _stack.RemoveAt(Size - 1);
        _popStatus = POP_OK;
    }

    public override void Clear()
    {
        _stack = [];

        _pushStatus = PUSH_NIL;
        _popStatus = POP_NIL;
        _peekStatus = PEEK_NIL;
    }

    public override T? Peek
    {
        get
        {
            T? result;
            if (Size == 0)
            {
                _peekStatus = PEEK_ERR;
                result = default;
            }
            else
            {
                _peekStatus = PEEK_OK;
                result = _stack.Last();
            }

            return result;
        }
    }

    public override int Size => _stack.Count;

    public override int PushStatus => _pushStatus;

    public override int PopStatus => _popStatus;

    public override int PeekStatus => _peekStatus;
}