import React from 'react'
import { useLoadingState } from 'leaf-validator'

export function Button(props) {
    const { children, whileProcessing, onClick, ...otherProps } = props;
    const [isProcessing, showProcessingWhile] = useLoadingState();

    const start = () => showProcessingWhile(onClick())

    return <button disabled={isProcessing} {...otherProps} onClick={start}>
        {isProcessing ? whileProcessing : children}
    </button>
}