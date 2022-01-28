/*
    NuGet Packer
    Author: James Keith - james.keith@tealium.com
    
*/

const fs = require('fs');
const readline = require('readline');
//const parser = require('xml2json');

const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

var config = {
    dirs: {
        current: __dirname,
        specs: "/Specs/",
        packages: "/Packages/"
    },
    log: {
        all: "log",
        debug: "log",
        warn: "warn",
        err: "error",
        enabled: true
    }
};
var c = config; //Alias

//Global Data to work with later.
var data = {
    files: [],
    specs: []
};
var d = data; //Alias

//Utility Functions.
var util = {
    log: function(lvl, val) {
        if (!c.log.enabled) { return; }

        if (typeof val === "object") {
            for (var i in val) {
                !!console[lvl] && console[lvl](val[i]);
            }
        } else {
            !!console[lvl] && console[lvl](val);
        }

    },
    filter: function(arr, str, opt) {
        var res = [];
        switch (opt || -1) {
            case this.filterOpts.STARTS_WITH:
                return arr.filter(val => val.indexOf(str) === 0);
                break;
            case this.filterOpts.ENDS_WITH:
                var regex = new RegExp(str + "$");
                return arr.filter(val => regex.test(val));
                break;
            case this.filterOpts.CONTAINS:
            default:
                return arr.filter(val => val.indexOf(str) > -1);
        }
    },
    filterOpts: {
        ENDS_WITH: 1,
        STARTS_WITH: 2,
        CONTAINS: 3
    },
    processes: {
        selectFile: function(spec) {
            u.log(c.log.debug, "Logging files:");
            //u.log(c.log.all, d.files);
            for (var i = 0; i < d.files.length; i++) {
                u.log(c.log.all, "[" + i + "] " + d.files[i]);
            }

            if (index) {
                u.processes.shouldIncrement(spec, true);
            } else {
                rl.question("Which spec did you want to package?", function(answer) {
                    u.log(c.log.debug, d.files[parseInt(answer)]);
                    u.log(c.log.debug, d.specs[parseInt(answer)]);
                    //rl.close();
                    //setTimeout(function() {
                    u.processes.shouldIncrement(d.specs[parseInt(answer)])
                        //}), 10;
                })
            }


        },
        shouldIncrement(spec, yes) {
            if (!!spec) {
                if (yes) {
                    u.log(c.log.debug, "Spec " + spec.id + " is currently version " + spec.version + ", would you like to increment it first? [y]es, [n]o");
                    u.processes.checkDependencies(spec);
                    spec.incrementVersion();
                    u.processes.packageSpec(spec);
                    u.log(c.log.debug, "Version Updated: " + spec.version);
                } else {
                    rl.question("Spec " + spec.id + " is currently version " + spec.version + ", would you like to increment it first? [y]es, [n]o", function(answer) {
                        u.log(c.log.debug, answer);
                        switch (answer) {
                            case "y":
                            case "yes":
                                //TODO
                                u.processes.checkDependencies(spec);
                                spec.incrementVersion();
                                u.log(c.log.debug, "Version Updated: " + spec.version);
                                break;
                            case "n":
                            case "no":
                                //TODO
                                u.log(c.log.debug, "Version Remains: " + spec.version);
                            default:

                                return;
                        }
                        //rl.close();
                        u.processes.packageSpec(spec);
                    })
                }
            }
        },
        checkDependencies: function(spec) {
            ///TODO: Code to check for dependencies.
            var res = [];
            for (var s in d.specs) {
                if (d.specs[s] != spec) {
                    for (var i = 0; i < d.specs[s].dependencies.length; i++) {
                        if (d.specs[s].dependencies[i].id == spec.id && d.specs[s].dependencies[i].version == spec.version) {
                            res.push(d.specs[s]);
                            u.log(c.log.warn, d.specs[s].id + " has dependency on this version; consider repackaging.")
                        }
                    }
                }
            }
            return res;
        },
        packageSpec: function(spec) {
            ///TODO: Code to run process to package spec.



        },
    }
}
var u = util; //alias

function Spec(path) {
    var self = this;

    this.path = path;

    this.id = "";
    this.version = "";
    this.dependencies = [];

    this.contents = fs.readFileSync(this.path, "UTF-8");
    u.log(c.log.debug, "Spec -> " + this.contents);

    var getStringField = function(name) {
        let fieldRegex = new RegExp("<" + name + ">([^<]+)<\/" + name + ">");
        let matches = fieldRegex.exec(self.contents);
        //u.log(c.log.debug, matches);
        return !!matches[1] ? matches[1] : "";
    }
    this.version = getStringField("version");
    this.id = getStringField("id");

    var getDependencies = function() {
        //<dependency id="([^"]+)" version="([^"]+)" \/>
        let dependRegex = new RegExp('<dependency id="([^"]+)" version="([^"]+)" \/>');
        let matches = dependRegex.exec(self.contents);
        var res = [];
        if (!!matches) {
            for (var i = 1; i + 1 < matches.length; i += 2) {
                res.push({
                    id: matches[i],
                    version: matches[i + 1]
                })
            }
        }
        return res;
    }

    this.dependencies = getDependencies();

}

Spec.prototype.incrementVersion = function(str) {
    if (str) {
        this.version = str;
    } else {
        var parts = this.version.split('.');
        parts[parts.length - 1] = parseInt(parts[parts.length - 1]) + 1;
        this.version = parts.join('.');
    }
}






u.log(c.log.all, "Welcome to the NuGet Packaging utility.");
u.log(c.log.debug, "Working in " + c.dirs.current);


d.files = fs.readdirSync(c.dirs.current + c.dirs.specs);
d.files = u.filter(data.files, ".nuspec", u.filterOpts.ENDS_WITH);
//d.files = u.filter(data.files, "droid");
for (var f in d.files) {
    u.log(c.log.debug, c.dirs.current + c.dirs.specs + d.files[f]);
    d.specs.push(new Spec(c.dirs.current + c.dirs.specs + d.files[f]))
}

u.processes.selectFile(d.specs[1]);