import styles from '../code/code_input.module.css';

function CodeInput({values, refs, onChange, onKeyDown}){
    return(
        <div className={styles.block_code}>
            {values.map((val, i) => (
                <input 
                    className={styles.square_input}
                    key={i}
                    ref={el => refs.current[i] = el}
                    value={val}
                    maxLength={1}
                    onChange={(e) => onChange(e.target.value.toUpperCase(), i)}
                    onKeyDown={(e) => onKeyDown(e, i)}
                />
            ))}
        </div>
    )
}

export default CodeInput;