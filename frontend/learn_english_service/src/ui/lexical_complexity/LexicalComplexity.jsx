import { getLvlClass } from '../../utils/get_lvl/getLvlClass';
import styles from '../lexical_complexity/lexical_complexity.module.css';

function LexicalComplexity({lexicalComplexity}){
    return(
        <div className={`${styles.lexical_complexity} ${getLvlClass(lexicalComplexity, styles)}`}>
            {lexicalComplexity}
        </div>
    )
}

export default LexicalComplexity;