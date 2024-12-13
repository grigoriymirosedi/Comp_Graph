// ќќќќќќќќќќќќќ WebGL
const canvas = document.getElementById('webglCanvas');
const gl = canvas.getContext('webgl');
if (!gl) {
    alert('WebGL ќќ ќќќќќќќќќќќќќќ ќ ќќќќќ ќќќќќќќќ.');
}
canvas.width = 570;  
canvas.height = 570;
// Улучшение качества изображения canvas
function resizeCanvasToDisplaySize(canvas) {
    const displayWidth = canvas.clientWidth;
    const displayHeight = canvas.clientHeight;
    const needResize =
        canvas.width !== displayWidth || canvas.height !== displayHeight;

    if (needResize) {
        canvas.width = displayWidth;
        canvas.height = displayHeight;
    }
    return needResize;
}
resizeCanvasToDisplaySize(canvas);
//canvas.width = window.innerWidth;
//canvas.height = window.innerHeight;
gl.viewport(0, 0, gl.canvas.width, gl.canvas.height);

// ќќќќќќќќќ ќќќќќќ
const vertexShaderSource = `
    attribute vec4 a_position;
    attribute vec2 a_texCoord;
    uniform mat4 u_modelViewMatrix;
    varying vec2 v_texCoord;

    void main() {
        gl_Position = u_modelViewMatrix * a_position;
        v_texCoord = a_texCoord;
    }
`;

// ќќќќќќќќќќќ ќќќќќќ
const fragmentShaderSource = `
    precision mediump float;
    varying vec2 v_texCoord;
    uniform sampler2D u_texture1;
    uniform sampler2D u_texture2;
    uniform float u_mixRatio;
    void main() {
        vec4 tex1 = texture2D(u_texture1, v_texCoord);
        vec4 tex2 = texture2D(u_texture2, v_texCoord);
        gl_FragColor = mix(tex1, tex2, u_mixRatio);
    }
`;

// ќќќќќќќ ќќќќќќќќќќ ќќќќќќќ
function compileShader(source, type) {
    const shader = gl.createShader(type);
    gl.shaderSource(shader, source);
    gl.compileShader(shader);
    if (!gl.getShaderParameter(shader, gl.COMPILE_STATUS)) {
        console.error('ќќќќќќ ќќќќќќќќќќ ќќќќќќќ:', gl.getShaderInfoLog(shader));
    }
    return shader;
}

// ќќќќќќќќ ќќќќќќќќќ
function createProgram(vertexShader, fragmentShader) {
    const program = gl.createProgram();
    gl.attachShader(program, vertexShader);
    gl.attachShader(program, fragmentShader);
    gl.linkProgram(program);
    if (!gl.getProgramParameter(program, gl.LINK_STATUS)) {
        console.error('ќќќќќќ ќќќќќќќќ ќќќќќќќќќ:', gl.getProgramInfoLog(program));
    }
    return program;
}

// ќќќќќќќќќќ ќќќќќќќќ ќ ќќќќќќќќ ќќќќќќќќќ
const vertexShader = compileShader(vertexShaderSource, gl.VERTEX_SHADER);
const fragmentShader = compileShader(fragmentShaderSource, gl.FRAGMENT_SHADER);
const program = createProgram(vertexShader, fragmentShader);

gl.useProgram(program);

// Define the cube's vertices and texture coordinates
const vertices = new Float32Array([
    // Front face
    -0.3, -0.3, -0.3,  0.0, 0.0, // Bottom-left
    0.3, -0.3, -0.3, 1.0, 0.0,  // Bottom-right
    0.3,  0.3, -0.3, 1.0, 1.0,  // Top-right
    -0.3,  0.3, -0.3, 0.0, 1.0, // Top-left

    // Back face
    -0.3, -0.3,  0.3, 0.0, 0.0, // Bottom-left
    0.3, -0.3,  0.3, 1.0, 0.0,  // Bottom-right
    0.3,  0.3,  0.3, 1.0, 1.0,  // Top-right
    -0.3,  0.3,  0.3, 0.0, 1.0, // Top-left

    // Top face
    -0.3,  0.3, -0.3, 0.0, 0.0, // Bottom-left
    0.3,  0.3, -0.3, 1.0, 0.0,  // Bottom-right
    0.3,  0.3,  0.3, 1.0, 1.0,  // Top-right
    -0.3,  0.3,  0.3, 0.0, 1.0, // Top-left

    // Bottom face
    -0.3, -0.3, -0.3, 0.0, 0.0, // Bottom-left
    0.3, -0.3, -0.3, 1.0, 0.0,  // Bottom-right
    0.3, -0.3,  0.3, 1.0, 1.0,  // Top-right
    -0.3, -0.3,  0.3, 0.0, 1.0, // Top-left

    // Right face
    0.3, -0.3, -0.3, 0.0, 0.0, // Bottom-left
    0.3,  0.3, -0.3, 1.0, 0.0,  // Bottom-right
    0.3,  0.3,  0.3, 1.0, 1.0,  // Top-right
    0.3, -0.3,  0.3, 0.0, 1.0, // Top-left

    // Left face
    -0.3, -0.3, -0.3, 0.0, 0.0, // Bottom-left
    -0.3,  0.3, -0.3, 1.0, 0.0,  // Bottom-right
    -0.3,  0.3,  0.3, 1.0, 1.0,  // Top-right
    -0.3, -0.3,  0.3, 0.0, 1.0  // Top-left
]);


const indices = new Uint16Array([
    0, 1, 2, 0, 2, 3,       // Front face
    4, 5, 6, 4, 6, 7,       // Back face
    8, 9, 10, 8, 10, 11,    // Top face
    12, 13, 14, 12, 14, 15, // Bottom face
    16, 17, 18, 16, 18, 19, // Right face
    20, 21, 22, 20, 22, 23  // Left face
]);

// Create buffers
const vbo = gl.createBuffer();
gl.bindBuffer(gl.ARRAY_BUFFER, vbo);
gl.bufferData(gl.ARRAY_BUFFER, vertices, gl.STATIC_DRAW);

const ibo = gl.createBuffer();
gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, ibo);
gl.bufferData(gl.ELEMENT_ARRAY_BUFFER, indices, gl.STATIC_DRAW);

const aPosition = gl.getAttribLocation(program, 'a_position');
const aTexCoord = gl.getAttribLocation(program, 'a_texCoord');
gl.vertexAttribPointer(aPosition, 3, gl.FLOAT, false, 5 * Float32Array.BYTES_PER_ELEMENT, 0);
gl.vertexAttribPointer(aTexCoord, 2, gl.FLOAT, false, 5 * Float32Array.BYTES_PER_ELEMENT, 3 * Float32Array.BYTES_PER_ELEMENT);
gl.enableVertexAttribArray(aPosition);
gl.enableVertexAttribArray(aTexCoord);

// ќќќќќќќќ ќќќќќќќќ
const texture1 = loadTexture('texture1.png');
const texture2 = loadTexture('texture2.png');

// Uniform locations
const uModelViewMatrix = gl.getUniformLocation(program, 'u_modelViewMatrix');
const uTexture1 = gl.getUniformLocation(program, 'u_texture1');
const uTexture2 = gl.getUniformLocation(program, 'u_texture2');
const uMixRatio = gl.getUniformLocation(program, 'u_mixRatio');

// Set up texture units
gl.activeTexture(gl.TEXTURE0);
gl.bindTexture(gl.TEXTURE_2D, texture1);
gl.uniform1i(uTexture1, 0);

gl.activeTexture(gl.TEXTURE1);
gl.bindTexture(gl.TEXTURE_2D, texture2);
gl.uniform1i(uTexture2, 1);

// Animation and interaction
let textureMix = 0.5;
let angle = 0;

let translation = { x: 0, y: 0, z: -2.5 };
let rotation = { x: 0, y: 0, z: 0 };

document.addEventListener('keydown', (event) => {
    switch (event.key) {
        case 'y':
            translation.y += 0.1;
            break;
        case 'Y':
            translation.y -= 0.1;
            break;
        case 'x':
            translation.x += 0.1;
            break;
        case 'X':
            translation.x -= 0.1;
            break;
        case 'z':
            translation.z += 0.1;
            break;
        case 'Z':
            translation.z -= 0.1;
            break;    
        case 'w': // ќќќќќќќ ќќ ќќќ X
            rotation.x += 5;
            break;
        case 's': // ќќќќќќќ ќќ ќќќ X
            rotation.x -= 5;
            break;
        case 'a': // ќќќќќќќ ќќ ќќќ Y
            rotation.y += 5;
            break;
        case 'd': // ќќќќќќќ ќќ ќќќ Y
            rotation.y -= 5;
            break;
        case '[': // ќќќќќќќќќќ ќќќќќќќќќ ќќќќќќќќќќ
            if (textureMix < 1) textureMix += 0.05;
            break;
        case ']': // ќќќќќќќќќќ ќќќќќќќќќ ќќќќќќќќќќ
            if (textureMix > 0) textureMix -= 0.05;
            break;
    }
    let mat41 = window.mat4;
    const modelViewMatrix = mat41.create();

    mat4.perspective(modelViewMatrix, Math.PI / 4, canvas.width / canvas.height, 0.1, 100);
    mat4.translate(modelViewMatrix, modelViewMatrix, [translation.x, translation.y, translation.z]);
    mat4.rotateX(modelViewMatrix, modelViewMatrix, rotation.x * Math.PI / 180);
    mat4.rotateY(modelViewMatrix, modelViewMatrix, rotation.y * Math.PI / 180);

    gl.uniformMatrix4fv(uModelViewMatrix, false, modelViewMatrix);
});

// ќќќќќќќќ ќќќќќќќќќ
function render() {
    gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);

    
    gl.uniform1f(uMixRatio, textureMix);

    //gl.bindTexture(gl.TEXTURE_2D, texture);
    //gl.uniform1i(uTexture, 0);

    gl.drawElements(gl.TRIANGLES, indices.length, gl.UNSIGNED_SHORT, 0);
    requestAnimationFrame(render);
}

gl.clearColor(0.0, 0.0, 0.0, 1.0);
gl.enable(gl.DEPTH_TEST);
render();
