module.exports = {
    extends: ['alloy', 'alloy/react', 'alloy/typescript'],
    env: {
        // 您的环境变量（包含多个预定义的全局变量）
        // Your environments (which contains several predefined global variables)
        //
        // browser: true,
        // node: true,
        // mocha: true,
        // jest: true,
        // jquery: true
    },
    globals: {
        // 您的全局变量（设置为 false 表示它不允许被重新赋值）
        // Your global variables (setting to false means it's not allowed to be reassigned)
        //
        // myGlobal: false
    },
    rules: {
        // 自定义您的规则
        // Customize your rules
    }
    // parser: '@typescript-eslint/parser',
    // plugins: ['@typescript-eslint'],
    // rules: {
    //     // 禁止使用 var
    //     'no-var': 'error',
    //     // 优先使用 interface 而不是 type
    //     '@typescript-eslint/consistent-type-definitions': ['error', 'interface']
    // }
};
