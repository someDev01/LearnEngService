import { BookMarkedIcon, Brain, ChevronDown, ChevronUpIcon, Clapperboard, SubtitlesIcon } from 'lucide-react';
import styles from '../oppotrunity_option/opportunity_option.module.css';

function OpportunityOption({index, openIndex, title, type, imgTag, content, onClick}){
    return(
        <div className={styles.option_block}>
            <div className={styles.option_trigger} onClick={onClick} style={{borderBottom: openIndex === index ? '1px solid rgb(48, 48, 48)' : ''}}>
                <div className={styles.option_title}>
                    <div className={styles.option_icon}>
                        {type === "clip" ? 
                            <Clapperboard size={22} color='#ff7f1e'/> :
                            type === "sub" ? 
                            <SubtitlesIcon size={22} color='#ff7f1e'/> : 
                            type === "dict" ?
                            <BookMarkedIcon size={22} color='#ff7f1e'/> :
                            <Brain size={22} color='#ff7f1e'/> }
                    </div>
                    <p>{title}</p>
                </div>
                <div className={styles.dropdown_button}>
                    {openIndex === index ? 
                        <ChevronUpIcon size={20} color='rgb(184, 184, 184)'/> : 
                        <ChevronDown size={20} color='rgb(184, 184, 184)'/>
                    }
                    
                </div>
            </div>
            <div className={styles.dropdown_content} style={{maxHeight: `${openIndex === index ? '300px' : '0px'}`}}>
                <p>{content}</p>
                <div className={styles.content_img}>
                    {imgTag}
                </div>
            </div>
        </div>
    )
}

export default OpportunityOption;