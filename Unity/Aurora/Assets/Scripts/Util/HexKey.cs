using System;
using System.Collections.Generic;


class HexKey : IEquatable<HexKey> {
    public int x;
    public int y;
    public int z;
    
    public HexKey(int x, int y, int z) 
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public override int GetHashCode() {
       return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
    }

    public override bool Equals(object obj) {
        return Equals(obj as HexKey);
    }

    public bool Equals(HexKey obj) {
        return obj != null 
        && obj.x == this.x
        && obj.y == this.y 
        && obj.z == this.z  ;
    }
}