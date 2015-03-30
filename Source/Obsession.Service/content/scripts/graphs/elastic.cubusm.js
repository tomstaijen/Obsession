cubism.elasticsearch = function (context, options, callback) {
    if (!context) throw new Error("Please pass a valid Cubism context instance as the first argument");

    options = options || {}
    if (!options.host) options.host = "http://localhost:9200";

    var source = {},
        context = context,
        initialized_metrics = {};

    source.cluster = {};
    source.node_names = {};

    // Returns a function, which will return data for the Cubism horizon chart callbacks.
    //
    var __cubism_metric_callback = function (node, expression) {
        return function (start, stop, step, callback) {
            var values = [],
                value = 0,
                start = +start,
                stop = +stop,
                metric_id = node.id + '-' + expression;

            d3.json(source.url(), function (data) {
                if (!data) return callback(new Error("Unable to load data from ElasticSearch!"));
                if (!data.nodes[node.id]) return callback(new Error("Unable to find node " + node.id + "!"));

                var value = eval("data.nodes['" + node.id + "']." + expression) // data.nodes[<NAME>].os.cpu.user
                // console.log(expression + ': ' + value)

                // Cubism.js expects a value for every "slot" based on the `start` and `stop` parameters, because
                // it assumes a backend such as [_Graphite_](https://github.com/square/cubism/wiki/Graphite),
                // which is able to return values stored over time.
                //
                // In _ElasticSearch_, we don't have any data stored: we poll the API repeatedly.
                //
                // On first call, Cubism.js calls the metric callback function with a large `start` and `stop` gap,
                // based on the `step` and `size` values of your chart. This would spoil the chart with a useless
                // "thick colored line".
                //
                // So: if we have already initialized this metric, push the same value to all the "slots",
                // because this is what Cubism.js expects...
                if (initialized_metrics[metric_id]) {
                    while (start < stop) {
                        start += step;
                        values.push(value);
                    }
                }
                    // ... otherwise mark this metric as initialized and fill the empty / "historical" slots with empty values.
                else {
                    initialized_metrics[metric_id] = true;
                    while (start < (stop - step)) {
                        start += step;
                        values.push(NaN);
                    }
                    values.push(value);
                }

                callback(null, values)
            });
        }
    }

    // Load information about ElasticSearch nodes from the Nodes Info API
    // [http://www.elasticsearch.org/guide/reference/api/admin-cluster-nodes-info.html]
    //
    d3.json(options.host + "/_cluster/nodes", function (cluster) {
        source.cluster = cluster
        source.node_names = d3.keys(cluster.nodes)

        // Returns a metric for specific node
        //
        // Arguments
        // ---------
        //
        // * `expression` -- A valid path in the ElasticSearch JSON response (eg. `os.cpu.user`)
        // * `n`          -- A node specification. Can be either node ID (eg. `USNEtnCWQW-5oG3Gf9J8Hg`),
        //                   or a number giving position in response (eg. `0`)
        //
        // For usage, see documentation for `cubism.elasticsearch`
        //
        source.metric = function (expression, n) {
            var n = n || 0,
                node_id = ("number" == typeof n) ? source.node_names[n] : n,
                node = source.cluster.nodes[node_id];

            var metric = context.metric(__cubism_metric_callback(node, expression), expression + " [" + node.name + "]");

            return metric;
        };

        // Returns a metric across all nodes
        //
        // Arguments
        // ---------
        //
        // * `expression` -- A valid path in the ElasticSearch JSON response (eg. `os.cpu.user`)
        //
        // For usage, see documentation for `cubism.elasticsearch`
        //
        source.metrics = function (expression) {
            var metrics = [],
                ordered_nodes = [];
            for (var n in source.cluster.nodes)
            { var o = source.cluster.nodes[n]; o.id = n; ordered_nodes.push(o) }

            ordered_nodes = ordered_nodes.sort(function (a, b) {
                if (a.name < b.name) return -1;
                if (a.name > b.name) return 1;
                return 0;
            });

            ordered_nodes.forEach(function (node) {
                var metric = context.metric(__cubism_metric_callback(node, expression), expression + " [" + node.name + "]");
                metrics.push(metric)
            })

            return metrics;
        };

        callback.call(source)
    })

    source.toString = function () { return options.host };
    source.url = function () { return options.host + "/_cluster/nodes/stats?all" };

    return source;
};