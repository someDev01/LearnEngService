import { useEffect, useRef, useState } from "react";

export function useCodeInput(length){

    const inputRefs = useRef([]);
    const [values, setValues] = useState(Array(length).fill(''));

    useEffect(() => {
        if(inputRefs.current[0]){
            inputRefs.current[0].focus();
        }
    }, []);

    const handleChange = (value, index) => {  
        
        const prevValues = [...values];
        const isCurrEmpty = values[index] === '';
        const isNextEmpty = values[index+1] === '';

        prevValues[index] = value;
        setValues(prevValues);

        if(isCurrEmpty && isNextEmpty && value !== '' && index < length)
            inputRefs.current[index+1]?.focus();
    };

    const onKeyDown = (e, index) => {
        if(e.key === 'Backspace'){

            if(values[index] !== '') return;

            if(index > 0) inputRefs.current[index-1]?.focus();
        }

        if(e.key === 'ArrowLeft' && index > 0)
            inputRefs.current[index-1]?.focus();

        if(e.key === 'ArrowRight' && index < length-1)
            inputRefs.current[index+1]?.focus();

        if(e.key === ' '){
            e.preventDefault();
            return;
        }
    }

    const code = values.join('');
    const isComplete = values.every(v => v !== '') && values.length === length;

    return{
        values,
        inputRefs,
        handleChange,
        onKeyDown,
        code,
        isComplete
    }
}