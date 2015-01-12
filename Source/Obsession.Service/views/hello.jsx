/** @jsx React.DOM */
// views/hello.jsx
var HelloWorld = React.createClass({
    render: function () {
        return (
            <div>Hello {this.props.firstName} {this.props.lastName}.</div>
        );
    }
});