interface Pointlike {
    x: number;
    y: number;
}
interface Named {
    name: string;
}

function logPoint(point: Pointlike): string {

    let result = "x = ".concat(point.x.toString(), ", y = ", point.y.toString());
    return result;
    //return "x = " + point.x.toString() + ", y = " + point.y.toString();
}

function logName(x: Named) {
    return "Hello there, how's things" + x.name + "?";
}

