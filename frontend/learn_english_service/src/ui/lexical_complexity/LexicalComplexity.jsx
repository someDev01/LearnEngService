import { getLvlClass } from '../../utils/get_lvl/getLvlClass';
import styles from '../lexical_complexity/lexical_complexity.module.css';

function LexicalComplexity({video}){
    return(
        <div className={`${styles.lexical_complexity} ${getLvlClass(video.lexicalComplexity, styles)}`}>
            {video.lexicalComplexity}
        </div>
    )
}

export default LexicalComplexity;