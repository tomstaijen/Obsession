/** @jsx React.DOM */
// views/hello.jsx
var HelloWorld = React.createClass({
    render: function () {
        return (
            <div>Hello World.</div>
        );
    }
});

React.render(<HelloWorld/>, document.getElementById('example'));