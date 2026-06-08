import styles from '../lexical_complexity/lexical_complexity.module.css';

function LexicalComplexity({lvl}){
    return(
        <div className={styles.lexical_complexity}>
            <p>{lvl}</p>
        </div>
    )
}

export default LexicalComplexity;